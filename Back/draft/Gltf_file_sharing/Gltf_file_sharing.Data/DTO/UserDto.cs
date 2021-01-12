using System;
using System.Collections.Generic;
using System.Text;

namespace Gltf_file_sharing.Data.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public UserDto(string email, string password, string name)
        {
            Email = email;
            Password = password;
            Name = name;
        }

        public UserDto()
        {
        }
    }
}
