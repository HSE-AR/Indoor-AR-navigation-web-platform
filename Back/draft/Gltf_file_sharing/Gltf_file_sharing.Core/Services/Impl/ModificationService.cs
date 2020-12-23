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
            var path = string.Empty;
            //подумать как вынести это в отедльную фукнцию, чтобы было применимо ко всем функциям
            switch (modification.ObjectType)
            {
                case ObjectTypes.Geometry:
                    path = "Scene.geometries";
                    break;

                case ObjectTypes.Material:
                    path = "Scene.materials";
                    break;

                case ObjectTypes.ObjectChildren:
                    path = "Scene.object.children";
                    break;

                case ObjectTypes.Object:
                    //сделать кастомное исключение, чтобы потом отлавливать в Middleware
                    throw new Exception("Недопустимая операция для данного элемента");

                default: break;
            }
            var update = Builders<BsonDocument>.Update.AddToSet(path, modification.Object);
            return await _models.UpdateOneAsync(filter, update);
        }

        private async Task<UpdateResult> UpdateModelElementsAsync(FilterDefinition<BsonDocument> filter, Modification modification)
        {
            var model = await _models.Find(filter).FirstOrDefaultAsync();
            var uuid = modification.Object["uuid"].ToString();
            var sectionName = GetSectionName(modification.ObjectType);
           
            if (sectionName == "materials" || sectionName == "geometries")
            {
                var section = model["Scene"][sectionName].AsBsonArray;
                var update = section.FirstOrDefault(x => x["uuid"] == uuid);
                var ind = section.IndexOf(update);
                section[ind] = modification.Object;
                model["Scene"][sectionName] = section;
            }
            else if (sectionName == "children")
            {
                var section = model["Scene"]["object"][sectionName].AsBsonArray;
                var update = section.FirstOrDefault(x => x["uuid"] == uuid);
                var ind = section.IndexOf(update);
                section[ind] = modification.Object;
                model["Scene"]["object"][sectionName] = section;
            }
            else if (sectionName == "object")
            {
                var childrens = model["Scene"][sectionName]["children"].AsBsonArray;
                model["Scene"][sectionName] = modification.Object;
                model["Scene"][sectionName]["children"] = childrens;
            }
            //переписать в свете добавления $set, чтобы переписывать отдельные свойства
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

                case ObjectTypes.ObjectChildren:
                    return "children";

                default:
                    throw new Exception("Invalid element");
            }
        }

        #endregion

    }
}
