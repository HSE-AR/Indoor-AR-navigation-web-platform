using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gltf_file_sharing.Data.Entities
{
    public class Modification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime EditedAtUtc { get; set; }

        public ModificationType Type { get; set; }

        public ObjectTypes ObjectType { get; set; }
        
        public BsonDocument Object { get; set; }

        public string ModelId { get; set; }
    }


    public enum ObjectTypes { Object, Material, Geometry };

    public enum ModificationType { Delete, Update, Add };
}
