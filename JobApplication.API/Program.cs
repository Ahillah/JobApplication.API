using JobApplication.API.Middleware;
using JobApplication.API.Services;
using JobApplication.Application.Interfaces.IRepositories;
using JobApplication.Application.Interfaces.IServices;
using JobApplication.Application.Services;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure;
using JobApplication.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Threading.RateLimiting;

namespace JobApplication.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            // Database
            var connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Identity
            builder.Services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;

                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // JWT Authentication
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration["Jwt:Key"]!))
                    };
                });

            // Rate Limiting
            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("auth", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        httpContext.Connection.RemoteIpAddress?.ToString()
                            ?? "unknown",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        }));

                options.RejectionStatusCode =
                    StatusCodes.Status429TooManyRequests;
            });

            // Application Services
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped(
    typeof(IGenericRepository<,>),
    typeof(GenericRepository<,>));
            builder.Services.AddScoped<
    IJobCandidateApplicationRepository,
    JobCandidateApplicationRepository>();
            builder.Services.AddScoped<
    IJobRepository,
    JobRepository>();
            builder.Services.AddScoped<
    IJobApplicationService,
    JobApplicationService>();
            builder.Services.AddScoped<
    IStorageService,
    StorageService>();
            builder.Services.AddScoped<IJobService, JobService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRateLimiter();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}