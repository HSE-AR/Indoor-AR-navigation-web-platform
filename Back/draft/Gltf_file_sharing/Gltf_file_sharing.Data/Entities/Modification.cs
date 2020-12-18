using Gltf_file_sharing.Data.DTO;
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

        public ModificationTypes Type { get; set; }

        public ObjectTypes ObjectType { get; set; }
        
        public BsonDocument Object { get; set; }

        public string ModelId { get; set; }

        public Modification(ModificationDto modificationDto)
        {
            Id = modificationDto.Id;
            EditedAtUtc = modificationDto.EditedAtUtc;
            Object = BsonDocument.Parse(modificationDto.Object.ToString());
            ModelId = modificationDto.ModelId;
            ObjectType = modificationDto.ObjectType;
            Type = modificationDto.Type;
        }
    }


    public enum ObjectTypes { Object, Material, Geometry, ObjectChildren };

    public enum ModificationTypes { Delete, Update, Add };
}
