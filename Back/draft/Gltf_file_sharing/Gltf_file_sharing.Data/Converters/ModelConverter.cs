using Gltf_file_sharing.Data.DTO;
using Gltf_file_sharing.Data.Entities;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gltf_file_sharing.Data.Converters
{
    public static class ModelConverter
    {
        public static ModelDto Convert(Model model) =>
            new ModelDto
            {
                Id = model.Id,
                Name = model.Name,
                Scene = JObject.Parse(model.Scene.ToJson()),
                CreatedAtUtc = model.CreatedAtUtc
            };

        public static Model Convert(ModelDto modelDto) =>
             new Model
             {
                 Id = modelDto.Id,
                 Name = modelDto.Name,
                 Scene = BsonDocument.Parse(modelDto.Scene.ToString()),
                 CreatedAtUtc = modelDto.CreatedAtUtc
             };

        public static ICollection<Model> Convert(ICollection<ModelDto> modelsDto) =>
            modelsDto.Select(x => Convert(x)).ToList();

        public static ICollection<ModelDto> Convert(ICollection<Model> models) =>
            models.Select(x => Convert(x)).ToList();
    }
}
