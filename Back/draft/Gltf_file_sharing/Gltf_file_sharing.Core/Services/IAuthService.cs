using Gltf_file_sharing.Data.DTO;
using System.Threading.Tasks;

namespace StudentResumes.AUTH.Interfaces
{
    public interface IAuthService
    {
        Task<object> Login(string email, string password);

        Task<object> Register(UserDto item);
    }
}
