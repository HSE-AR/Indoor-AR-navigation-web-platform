using Gltf_file_sharing.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gltf_file_sharing.Core.Helpers
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            string superAdminEmail = "super@admin.com";
            string password = "superadmin123";

            if (await userManager.FindByEmailAsync(superAdminEmail) == null)
            {
                User superAdmin = new User { Email = superAdminEmail, UserName = superAdminEmail };
                IdentityResult result = await userManager.CreateAsync(superAdmin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, "superadmin");
                }
            }
        }
    }
}
