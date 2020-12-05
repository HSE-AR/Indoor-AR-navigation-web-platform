using System;
using System.Collections.Generic;
using System.Text;

namespace Gltf_file_sharing.Data.Settings
{
    public class ModelsDatabaseSettings : IModelsDatabaseSettings
    { 
        public string ModelsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }


    public interface IModelsDatabaseSettings
    {
        string ModelsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
