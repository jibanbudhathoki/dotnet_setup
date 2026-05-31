using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using rms_backend.Features.Auth.Commands.Login;
using rms_backend.Features.Auth.Commands.Register;
using rms_backend.Features.Auth.Services;
using rms_backend.Infrastructure.Persistence;
using rms_backend.Infrastructure.Repositories;
using rms_backend.Shared.Constants;

namespace rms_backend.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var secret = config[AppConstants.JwtSecretKey]
            ?? throw new InvalidOperationException("JWT secret is not configured.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config[AppConstants.JwtIssuerKey],
                    ValidAudience = config[AppConstants.JwtAudienceKey],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
            });

        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Auth Services
        services.AddScoped<IJwtService, JwtService>();

        // Handlers
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();

        // Validators
        services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

        return services;
    }
}
