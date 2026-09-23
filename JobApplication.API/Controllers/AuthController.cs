using JobApplication.Application.DTO_s.Identity;
using JobApplication.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace JobApplication.API.Controllers
{
    /// <summary>
    /// Handles user authentication operations, including registration and login.
    /// </summary>
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

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="dto">
        /// The registration data including the user's personal information,
        /// email, and password.
        /// </param>
        /// <returns>
        /// The created user's authentication information and JWT token.
        /// </returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="dto">
        /// The login credentials including email and password.
        /// </param>
        /// <returns>
        /// The authenticated user's information and JWT token.
        /// </returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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