using Gltf_file_sharing.Core.Databases;
using Gltf_file_sharing.Core.Repositories;
using Gltf_file_sharing.Data.DTO;
using Gltf_file_sharing.Data.Entities;
using Gltf_file_sharing.Data.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Core.Services
{
    public class ModificationService : IModificationService
    {
        private readonly ModificationRepository _modificationRepository;
        //нужен прямой доступ к коллекции (а не к репозиторию), для экономии памяти и времени 
        private readonly IMongoCollection<BsonDocument> _models;


        public ModificationService(MongoContext context, ModificationRepository modRep)
        {
            _modificationRepository = modRep;
            _models = context.ModelsAsBsonDocument;
        }

        public async Task<IEnumerable<ModificationDto>> GetAsync() =>
            (await _modificationRepository.GetAsync()).Select(x => new ModificationDto(x));

        public async Task<bool> ModifyModel(ModificationDto modificationDto)
        {
            Modification mod = new Modification(modificationDto);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(mod.ModelId));
            UpdateResult result;

            switch (mod.Type)
            {
                case ModificationTypes.Add:
                    result = await AddElementsToModelAsync(filter, mod);
                    break;

                case ModificationTypes.Delete:
                    result = await DeleteElementsFromModel(filter, mod);
                    break;

                case ModificationTypes.Update:
                    result = await UpdateModelElementsAsync(filter, mod);
                    break;

                default: 
                    result = null;
                    break;
            }

            if (result == null || !result.IsAcknowledged)
                return false;

            await _modificationRepository.CreateAsync(mod);
            return true;
        }

        public async Task<bool> ModifyModels(IEnumerable<ModificationDto> modificationDtos)
        {
            bool result = true;
            foreach (var moddto in modificationDtos)
                result &= await ModifyModel(moddto);

            return result;
        }

        #region private methods

        private async Task<UpdateResult> AddElementsToModelAsync(FilterDefinition<BsonDocument> filter, Modification modification)
        {
            if  (modification.ObjectType == ObjectTypes.Object)
            //сделать кастомное исключение, чтобы потом отлавливать в Middleware
                throw new Exception("Недопустимая операция для данного элемента");



            if (modification.Material == null || modification.Geometry == null ||
                                                        modification.ObjectChild == null)

                throw new Exception("Для добавления объекта должны быть не нулевыми " +
                                                       "поля Material, Geometry, ObjectChildren");

            var update = Builders<BsonDocument>.Update
                .AddToSet("Scene.object.children", modification.ObjectChild)
                .AddToSet("Scene.materials", modification.Material)
                .AddToSet("Scene.geometries", modification.Geometry);

            return await _models.UpdateOneAsync(filter, update);
        }

        private async Task<UpdateResult> UpdateModelElementsAsync(FilterDefinition<BsonDocument> filter, Modification modification)
        {
            var model = await _models.Find(filter).FirstOrDefaultAsync();
            
            var sectionName = GetSectionName(modification.ObjectType);
           

            if (modification.PropertyModificationType == ModificationTypes.Add ||
                modification.PropertyModificationType == ModificationTypes.Update)
                AddOrUpdateProperties(ref model, modification, sectionName);

            else if (modification.PropertyModificationType == ModificationTypes.Delete) 
                RemoveProperty(ref model, modification, sectionName);
           

            var result =  _models.UpdateOne(filter, new BsonDocument("$set", model));
            return result;
        }

        private async Task<UpdateResult> DeleteElementsFromModel(FilterDefinition<BsonDocument> filter, Modification modification)
        {
            
            var model = await _models.Find(filter).FirstOrDefaultAsync();
            var uuid = modification.Object["uuid"].ToString();
            var sectionName = GetSectionName(modification.ObjectType);

            BsonArray section;

            if (sectionName == "geometries" || sectionName == "materials")
                section = model["Scene"][sectionName].AsBsonArray;
            else
                section = model["Scene"]["object"][sectionName].AsBsonArray;

            var update = section.FirstOrDefault(x => x["uuid"] == uuid);
            section.Remove(update);

            if (sectionName == "geometries" || sectionName == "materials")
                model["Scene"][sectionName] = section;
            else
                model["Scene"]["object"][sectionName] = section;

            return await _models.UpdateOneAsync(filter, new BsonDocument("$set", model));

        } 

        private BsonDocument AddOrUpdateProperties(ref BsonDocument model, Modification modification, string sectionName)
        {
            string uuid = string.Empty;
            switch (sectionName)
            {
                case "materials":
                    UpdateMaterialProperties(ref model, modification, sectionName);
                    break;

                case "geometries":
                    UpdateGeometryProperties(ref model, modification, sectionName);
                    break;

                case "children":
                    UpdateObjectChildProperties(ref model, modification, sectionName);
                    break;

                case "object":
                    UpdateObjectProperties(ref model, modification, sectionName);
                    break;
                default:
                    throw new Exception("Недопустимое имя");
            }

            return model;
        }

        private BsonDocument RemoveProperty(ref BsonDocument model, Modification modification, string sectionName)
        {
            string uuid = string.Empty;
            switch (sectionName)
            {
                case "materials":
                    RemoveMaterialProperty(ref model, modification, sectionName);
                    break;
                case "geometries":
                    RemoveGeometryProperty(ref model, modification, sectionName);
                    break;

                case "children":
                    RemoveObjectChildProperty(ref model, modification, sectionName);
                    break;

                case "object":
                    RemoveObjectProperty(ref model, modification, sectionName);
                    break;
                default:
                    throw new Exception("Недопустимое имя");
            }

            return model;
        }

        private string GetSectionName(ObjectTypes type)
        {
            switch (type)
            {
                case ObjectTypes.Geometry:
                    return "geometries";

                case ObjectTypes.Material:
                    return "materials";

                case ObjectTypes.Object:
                    return "object";

                case ObjectTypes.ObjectChild:
                    return "children";

                default:
                    throw new Exception("Invalid element");
            }
        }

        private void UpdateMaterialProperties(ref BsonDocument model, Modification modification, string sectionName)
        {
            string uuid = string.Empty;
            var section = model["Scene"][sectionName].AsBsonArray;
            uuid = modification.Material["uuid"].ToString();
            var update = section.FirstOrDefault(x => x["uuid"] == uuid);
            var ind = section.IndexOf(update);

            foreach (var property in modification.Material.Names)
            {
                section[ind][property] = modification.Material[property];
            }

            model["Scene"][sectionName] = section;
        }

        private void UpdateGeometryProperties(ref BsonDocument model, Modification modification, string sectionName)
        {
            string uuid = string.Empty;
            var section = model["Scene"][sectionName].AsBsonArray;
            uuid = modification.Geometry["uuid"].ToString();
            var update = section.FirstOrDefault(x => x["uuid"] == uuid);
            var ind = section.IndexOf(update);

            foreach (var property in modification.Geometry.Names)
            {
                section[ind][property] = modification.Geometry[property];
            }

            model["Scene"][sectionName] = section;
        }

        private void UpdateObjectChildProperties(ref BsonDocument model, Modification modification, string sectionName)
        {
            var section = model["Scene"]["object"][sectionName].AsBsonArray;
            var uuid = modification.ObjectChild["uuid"].ToString();
            var update = section.FirstOrDefault(x => x["uuid"] == uuid);
            var ind = section.IndexOf(update);
            foreach (var property in modification.ObjectChild.Names)
            {
                section[ind][property] = modification.ObjectChild[property];
            }
            model["Scene"]["object"][sectionName] = section;
        }

        private void UpdateObjectProperties(ref BsonDocument model, Modification modification, string sectionName)
        {
            foreach (var property in modification.Object.Names)
            {
                model["Scene"][sectionName][property] = modification.Object[property];
            }
        }

        private void RemoveMaterialProperty(ref BsonDocument model, Modification modification, string sectionName)
        {
            var section = model["Scene"][sectionName].AsBsonArray;
            var uuid = modification.Material["uuid"].ToString();
            var update = section.FirstOrDefault(x => x["uuid"] == uuid);
            var ind = section.IndexOf(update);

            foreach (var property in modification.Material.Names)
            {
                section[ind].AsBsonDocument.Remove(property);
            }
            model["Scene"][sectionName] = section;
        }

        private void RemoveGeometryProperty(ref BsonDocument model, Modification modification, string sectionName)
        {
            var section = model["Scene"][sectionName].AsBsonArray;
            var uuid = modification.Geometry["uuid"].ToString();
            var update = section.FirstOrDefault(x => x["uuid"] == uuid);
            var ind = section.IndexOf(update);

            foreach (var property in modification.Geometry.Names)
            {
                section[ind].AsBsonDocument.Remove(property);
            }

            model["Scene"][sectionName] = section;
        }

        private void RemoveObjectChildProperty(ref BsonDocument model, Modification modification, string sectionName)
        {
            var section = model["Scene"]["object"][sectionName].AsBsonArray;
            var uuid = modification.ObjectChild["uuid"].ToString();
            var update = section.FirstOrDefault(x => x["uuid"] == uuid);
            var ind = section.IndexOf(update);
            foreach (var property in modification.ObjectChild.Names)
            {
                section[ind].AsBsonDocument.Remove(property);
            }
            model["Scene"]["object"][sectionName] = section;
        }

        private void RemoveObjectProperty(ref BsonDocument model, Modification modification, string sectionName)
        {
            foreach (var property in modification.Object.Names)
            {
                model["Scene"][sectionName].AsBsonDocument.Remove(property);
            }
        }
        #endregion

    }
}
