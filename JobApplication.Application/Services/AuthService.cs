using JobApplication.Application.DTO_s.Identity;
using JobApplication.Application.Interfaces.IServices;
using JobApplication.Domain.Constants;
using JobApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
     
        private readonly IJwtService _jwtService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
               SignInManager<ApplicationUser> signInManager,
                IJwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser is not null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Errors = new()
            {
                "البريد الإلكتروني مسجل بالفعل."
            }
                };
            }

            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(
                user,
                dto.Password);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Errors = result.Errors
                        .Select(e => e.Description)
                        .ToList()
                };
            }

            var roleResult = await _userManager.AddToRoleAsync(
                user,
                Roles.Candidate);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Errors = roleResult.Errors
                        .Select(e => e.Description)
                        .ToList()
                };
            }

            var token = await _jwtService.CreateTokenAsync(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = Roles.Candidate
            };
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Errors = new()
            {
                "البريد الإلكتروني أو كلمة المرور غير صحيحة."
            }
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                dto.Password,
                lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Errors = new()
            {
                "تم قفل الحساب مؤقتًا بسبب محاولات دخول متكررة."
            }
                };
            }

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Errors = new()
            {
                "البريد الإلكتروني أو كلمة المرور غير صحيحة."
            }
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault();

            var token = await _jwtService.CreateTokenAsync(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role
            };
        }
    }
}
