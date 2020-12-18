using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gltf_file_sharing.Data.Entities
{
    public class Model
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


        public string Name { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public BsonDocument Scene { get; set; }
        
    }
    
}
