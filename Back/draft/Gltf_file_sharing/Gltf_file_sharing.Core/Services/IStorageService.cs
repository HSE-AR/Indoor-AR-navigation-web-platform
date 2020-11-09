using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Core.Services
{
    public interface IStorageService
    {
        Task<string> UploadAsync(IFormFile file, string fileName, string storagePrefix);

        void RemoveFile(string fileName);

        void RemoveFileByFullPath(string filePath);
    }
}
