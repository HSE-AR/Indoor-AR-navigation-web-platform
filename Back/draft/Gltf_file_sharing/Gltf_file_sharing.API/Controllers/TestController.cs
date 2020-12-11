﻿using Gltf_file_sharing.Data.DTO;
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
        private readonly IMongoCollection<Model> _models;

        public TestController()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("ModelsDb");
            _models = _database.GetCollection<Model>("models");
        }
        [HttpPost]
        public ActionResult<bool> ModificationAdd([FromBody] ModificationDto modificationDto)
        {
            var model = _models.Find(x => x.Id == modificationDto.ModelId);

            
            return false;
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
