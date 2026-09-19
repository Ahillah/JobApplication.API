
using JobApplication.Application.DTO_s.Identity;
using JobApplication.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace JobApplication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(
                    ApiResponse<string>.Failure(
                        errors,
                        "توجد أخطاء في البيانات المدخلة."));
            }

            var result = await _authService.RegisterAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(
                    ApiResponse<AuthResponseDto>.Failure(
                        result.Errors,
                        "فشل إنشاء الحساب."));
            }

            return Ok(
                ApiResponse<AuthResponseDto>.Success(
                    result,
                    "تم إنشاء الحساب بنجاح."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(
                    ApiResponse<string>.Failure(
                        errors,
                        "بيانات الدخول غير صحيحة."));
            }

            var result = await _authService.LoginAsync(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(
                    ApiResponse<AuthResponseDto>.Failure(
                        result.Errors,
                        "فشل تسجيل الدخول."));
            }

            return Ok(
                ApiResponse<AuthResponseDto>.Success(
                    result,
                    "تم تسجيل الدخول بنجاح."));
        }
    }
}
