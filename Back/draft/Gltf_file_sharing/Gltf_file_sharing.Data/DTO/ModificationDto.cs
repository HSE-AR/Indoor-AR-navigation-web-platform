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

        public JObject Geometry { get; set; }

        public JObject Material { get; set; }

        public JObject ObjectChild { get; set; }

        public string ModelId { get; set; }

        public ModificationDto()
        {

        }

        public ModificationDto(Modification modification)
        {
            Id = modification.Id;
            EditedAtUtc = modification.EditedAtUtc;

            if (modification.Object != null)
                Object = JObject.Parse(modification.Object.ToJson());

            if (modification.Geometry != null)
                Geometry = JObject.Parse(modification.Geometry.ToJson());

            if (modification.ObjectChild != null)
                ObjectChild = JObject.Parse(modification.ObjectChild.ToJson());

            if (modification.Material != null)
                Material = JObject.Parse(modification.Material.ToJson());

            ModelId = modification.ModelId;
            ObjectType = modification.ObjectType;
            Type = modification.Type;
        }
    }
}