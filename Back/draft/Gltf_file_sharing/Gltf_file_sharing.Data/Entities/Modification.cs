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

        public ModificationTypes PropertyModificationType { get; set; }

        public BsonDocument Geometry { get; set; }

        public BsonDocument Material { get; set; }

        public BsonDocument ObjectChild { get; set; }
        
        public BsonDocument Object { get; set; }

        public string ModelId { get; set; }

        public Modification(ModificationDto modificationDto)
        {
            Id = modificationDto.Id;
            EditedAtUtc = modificationDto.EditedAtUtc;

            if (modificationDto.Object != null)
                Object = BsonDocument.Parse(modificationDto.Object.ToString());

            if (modificationDto.Geometry != null)
                Geometry = BsonDocument.Parse(modificationDto.Geometry.ToString());

            if (modificationDto.ObjectChild != null)
                ObjectChild = BsonDocument.Parse(modificationDto.ObjectChild.ToString());

            if (modificationDto.Material != null)
                Material = BsonDocument.Parse(modificationDto.Material.ToString());
            ModelId = modificationDto.ModelId;
            ObjectType = modificationDto.ObjectType;
            Type = modificationDto.Type;
            PropertyModificationType = modificationDto.PropertyModificationType;
        }
    }


    public enum ObjectTypes { Object, Material, Geometry, ObjectChild };

    public enum ModificationTypes { Delete, Update, Add };
}
