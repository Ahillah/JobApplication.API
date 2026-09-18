namespace JobApplication.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var (statusCode, message, isWarning) = ex switch
                {
                    KeyNotFoundException => (StatusCodes.Status404NotFound, ex.Message, true),
                    InvalidOperationException => (StatusCodes.Status400BadRequest, ex.Message, true),
                    UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, ex.Message, true),
                    ArgumentException => (StatusCodes.Status400BadRequest, ex.Message, true),
                    _ => (
                        StatusCodes.Status500InternalServerError,
                        _env.IsDevelopment()
                            ? ex.Message
                            : "حدث خطأ غير متوقع في النظام، يرجى المحاولة مرة أخرى لاحقاً.",
                        false)
                };

                if (isWarning)
                    _logger.LogWarning(ex, ex.Message);
                else
                    _logger.LogError(ex, "Unhandled exception for {Path}", context.Request.Path);

                var errors = new List<string> { message };
                if (_env.IsDevelopment() && ex.InnerException is not null)
                    errors.Add(ex.InnerException.Message);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<string>.Failure(errors, message));
            }
        }

    }
}