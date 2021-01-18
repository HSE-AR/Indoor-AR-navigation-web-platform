using Gltf_file_sharing.Core.EF;
using Gltf_file_sharing.Data.DTO;
using Gltf_file_sharing.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using StudentResumes.AUTH.Interfaces;
using System;
using System.Threading.Tasks;

namespace StudentResumes.AUTH.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtGenerator _jwt;

        public AuthService(SignInManager<User> sim, UserManager<User> um, IJwtGenerator jwt)
        {
            _signInManager = sim;
            _userManager = um;
            _jwt = jwt;
        }

        public async Task<object> Login(string email, string password)
        {
            if (email == null || password == null)
                throw new Exception("Invalid login or password");

            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (!result.Succeeded)
                throw new Exception("Something went wrong during registration");

            var appUser = await _userManager.FindByEmailAsync(email);

            return await _jwt.GenerateJwt(appUser);

        }

        public async Task<object> Register(UserDto item)
        {
            var user = new User(item);
            if (user == null)
                throw new ArgumentNullException();

            var result = await _userManager.CreateAsync(user, item.Password);

            if (!result.Succeeded)
                throw new Exception();

            await _userManager.AddToRoleAsync(user, "admin");
            await _signInManager.SignInAsync(user, false);
            return await _jwt.GenerateJwt(user);

        }
    }
}
