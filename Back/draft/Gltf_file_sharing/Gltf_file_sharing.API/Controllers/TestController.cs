using Gltf_file_sharing.Data.DTO;
using Gltf_file_sharing.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gltf_file_sharing.API.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
       
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoDatabase _testDatabase;
        private readonly IMongoCollection<BsonDocument> _models;

        public TestController()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("ModelsDb");
            _testDatabase = _client.GetDatabase("test");
            _models = _database.GetCollection<BsonDocument>("Models");
        }
        
        [HttpPost]
        public ActionResult Modification([FromBody] ModificationDto modificationDto)
        {

            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(modificationDto.ModelId));

            var model = _models.Find(filter).FirstOrDefault();

            string name = string.Empty;

            var uuid = modificationDto.Object["uuid"].ToString();

            if (modificationDto.ObjectType == ObjectTypes.Geometry)
                name = "geometries";
            else if (modificationDto.ObjectType == ObjectTypes.Material)
                name = "materials";
            else if (modificationDto.ObjectType == ObjectTypes.Object)
                name = "object";
            else if (modificationDto.ObjectType == ObjectTypes.ObjectChildren)
                name = "children";
            
            if(name == "geometries" || name == "materials")
            {
                if (modificationDto.Type == ModificationTypes.Add)
                {
                    var update = Builders<BsonDocument>.Update.AddToSet("Scene." + name, BsonDocument.Parse(modificationDto.Object.ToString()));
                    return new JsonResult(_models.UpdateOne(filter, update));
                }

                if (modificationDto.Type == ModificationTypes.Update)
                {
                    //получить uuid изменяего компонента
                    //изменить его полностью

                    //var filter1 = new BsonDocument {
                    //    { "_id", ObjectId.Parse(modificationDto.ModelId) },
                    //    { "Scene", 
                    //        new BsonDocument {
                    //            { name, new BsonArray(new[] { new BsonDocument { { "uuid", uuid } } }) } }
                    //    }
                    //};
                    //_models.FindOneAndUpdate(filter1, BsonDocument.Parse(modificationDto.Object.ToString()));

                    //что за костыли!!!!
                    var names = model["Scene"][name].AsBsonArray;
                    var update = names.FirstOrDefault(x => x["uuid"] == uuid);
                    var ind = names.IndexOf(update);
                    names[ind] = BsonDocument.Parse(modificationDto.Object.ToString());
                    model["Scene"][name] = names;
                    _models.UpdateOne(filter, model);
                }

                if (modificationDto.Type == ModificationTypes.Delete)
                {
                    var names = model["Scene"][name].AsBsonArray;
                    var update = names.FirstOrDefault(x => x["uuid"] == uuid);
                    names.Remove(update);
                    model["Scene"][name] = names;
                    _models.FindOneAndUpdate(filter, model);
                }
            }
            
            //при  ObjectType == ObjectChildren добавляем или изменяем объект в childrens, 
            //при  ObjectType == Object изменяем свойства Object, Add и Delete недоступен
            //childrens при этом не передаём, чтобы много памяти не тратить
            //более глубокую вложенность пока не рассматриваем
           if (name == "object")
            {
                if (modificationDto.Type == ModificationTypes.Delete || modificationDto.Type == ModificationTypes.Add)
                    return BadRequest("Недоступное действие для элемента данного типа");

                var childrens = model["Scene"][name]["children"].AsBsonArray;
                model["Scene"][name] = BsonDocument.Parse(modificationDto.Object.ToString());
                model["Scene"][name]["children"] = childrens;
                _models.FindOneAndUpdate(filter, model);
            }

           if (name == "children")
            {
                if (modificationDto.Type == ModificationTypes.Add)
                {
                    var update = Builders<BsonDocument>.Update.AddToSet("Scene.object." + name, BsonDocument.Parse(modificationDto.Object.ToString()));
                    return new JsonResult(_models.UpdateOne(filter, update));
                }
                else if (modificationDto.Type == ModificationTypes.Update)
                {

                    var names = model["Scene"]["object"][name].AsBsonArray;
                    var update = names.FirstOrDefault(x => x["uuid"] == uuid);
                    var ind = names.IndexOf(update);
                    names[ind] = BsonDocument.Parse(modificationDto.Object.ToString());
                    model["Scene"]["object"][name] = names;
                    _models.FindOneAndUpdate(filter, model);
                }
                else if (modificationDto.Type == ModificationTypes.Delete)
                {
                    var names = model["Scene"]["object"][name].AsBsonArray;
                    var update = names.FirstOrDefault(x => x["uuid"] == uuid);
                    names.Remove(update);
                    model["Scene"]["object"][name] = names;
                    _models.FindOneAndUpdate(filter, model);
                }
            }

           //создать репозиторий ModificationRepository
            //создать ModificationService для фукнций внесения измениений
            //создать новый контроллер
            
            return new JsonResult("kek");
        }

        [HttpGet]
        public async Task<string> GetTestString()
        {
            var modelsBson = _testDatabase.GetCollection<Model>("models");
            
            var filter = new BsonDocument();
            var cursor = await modelsBson.FindAsync(filter);
            cursor.MoveNextAsync();

            var people = cursor.Current;
            
            return people.First().Scene.ToJson();
        }
        
        //[HttpGet]
        //public ActionResult<string> Get()
        //{
        //    try
        //    {
        //        return _collection.Find(x => true).ToList().ToJson();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex);
        //    }
        //}

        //[HttpPost]
        //public ActionResult Post([FromBody]JObject doc)
        //{
        //    try
        //    {
        //        BsonDocument document = BsonDocument.Parse(doc.ToString());
        //        _collection.InsertOne(document);
        //        return Ok();
        //    }
        //    catch(Exception ex)
        //    {
        //        return StatusCode(500, ex);
        //    }
        //}



    }
}
