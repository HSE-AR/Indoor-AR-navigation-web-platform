using Gltf_file_sharing.Data.Entities;
using System.Threading.Tasks;

namespace StudentResumes.AUTH
{
    public interface IJwtGenerator
    {
        Task<object> GenerateJwt(User user);
    }
}
