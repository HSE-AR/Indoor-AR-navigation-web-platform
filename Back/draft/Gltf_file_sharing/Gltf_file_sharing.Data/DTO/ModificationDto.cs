using Gltf_file_sharing.Data.Entities;
using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gltf_file_sharing.Data.DTO
{
    public class ModificationDto
    {
        public string Id { get; set; }

        public DateTime EditedAtUtc { get; set; }

        public ModificationTypes Type { get; set; }

        public ObjectTypes ObjectType { get; set; }

        public JObject Object { get; set; }

        public string ModelId { get; set; }

        public ModificationDto()
        {

        }

        public ModificationDto(Modification modification)
        {
            Id = modification.Id;
            EditedAtUtc = modification.EditedAtUtc;
            Object = JObject.Parse(modification.Object.ToJson());
            ModelId = modification.ModelId;
            ObjectType = modification.ObjectType;
            Type = modification.Type;
        }
    }
}