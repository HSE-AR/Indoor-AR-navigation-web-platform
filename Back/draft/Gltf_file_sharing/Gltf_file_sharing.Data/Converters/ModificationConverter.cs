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
    public static class ModificationConverter
    {
        //public static Modification Convert(ModificationDto modificationDto) =>
        //    new Modification
        //    {
        //        Id = modificationDto.Id,
        //        EditedAtUtc = modificationDto.EditedAtUtc,
        //        Object = BsonDocument.Parse(modificationDto.Object.ToString()),
        //        ModelId = modificationDto.ModelId,
        //        ObjectType = modificationDto.ObjectType,
        //        Type = modificationDto.Type
        //    };

        //public static ModificationDto Convert(Modification modification) =>
        //    new ModificationDto
        //    {
        //        Id = modification.Id,
        //        EditedAtUtc = modification.EditedAtUtc,
        //        Object = JObject.Parse(modification.Object.ToJson()),
        //        ModelId = modification.ModelId,
        //        ObjectType = modification.ObjectType,
        //        Type = modification.Type
        //    };

        //public static ICollection<Modification> Convert(ICollection<ModificationDto> modificationsDto) =>
        //   modificationsDto.Select(x => Convert(x)).ToList();

        //public static ICollection<ModificationDto> Convert(ICollection<Modification> modifications) =>
        //    modifications.Select(x => Convert(x)).ToList();
    }
}
