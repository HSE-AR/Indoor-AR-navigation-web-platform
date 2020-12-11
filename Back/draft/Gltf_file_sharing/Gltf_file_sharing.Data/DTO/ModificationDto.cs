using Gltf_file_sharing.Data.Entities;
using Microsoft.VisualBasic.CompilerServices;
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

        public ModificationType Type { get; set; }

        public ObjectTypes ObjectType { get; set; }

        public JObject Object { get; set; }

        public string ModelId { get; set; }
    }
}