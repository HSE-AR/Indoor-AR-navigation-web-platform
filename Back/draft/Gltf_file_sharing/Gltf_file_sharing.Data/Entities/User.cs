using Gltf_file_sharing.Data.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gltf_file_sharing.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }

        public User(UserDto item)
        {
            Name = item.Name;
            Email = item.Email;
            UserName = item.Email;
        }
        public User()
        {

        }
    }
}
