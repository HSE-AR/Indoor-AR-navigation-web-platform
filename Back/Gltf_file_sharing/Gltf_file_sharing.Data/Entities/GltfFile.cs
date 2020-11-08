using System;
using System.Collections.Generic;
using System.Text;

namespace Gltf_file_sharing.Data.Entities
{
    public class GltfFile
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public DateTime CreationDateTime { get; set; }

        public DateTime LastUpdateDateTime { get; set; }

        public GltfFile(string name, DateTime creationDateTime)
        {
            Name = name;
            CreationDateTime = creationDateTime;
            LastUpdateDateTime = creationDateTime;
        }
    }
}
