using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gltf_file_sharing.Data.DTO
{
    public class ModelDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public JObject Scene { get; set; }
    }
}
