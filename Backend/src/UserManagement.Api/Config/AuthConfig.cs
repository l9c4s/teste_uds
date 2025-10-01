using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Domain.Enums;

namespace UserManagement.Api.Config;

public static class AuthConfig
{
    public static IServiceCollection AddTokenService(this IServiceCollection services, IConfiguration configuration)
    {

        // JWT Authentication
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

        
        services.AddScoped<IAuthorizationHandler, MinimumAccessLevelHandler>();


        services.AddAuthorization(options =>
               {
                   // Para cada nível de acesso, criamos uma política que usa nosso requisito personalizado.
                   // Um usuário Admin (nível 1) passará na política "CommonUser" (nível 6), pois 1 <= 6.
                   options.AddPolicy(nameof(AccessLevel.Administrator), policy => policy.AddRequirements(new MinimumAccessLevelRequirement(AccessLevel.Administrator)));
                   options.AddPolicy(nameof(AccessLevel.Manager), policy => policy.AddRequirements(new MinimumAccessLevelRequirement(AccessLevel.Manager)));
                   options.AddPolicy(nameof(AccessLevel.CommonUser), policy => policy.AddRequirements(new MinimumAccessLevelRequirement(AccessLevel.CommonUser)));
                   options.AddPolicy(nameof(AccessLevel.Viewer), policy => policy.AddRequirements(new MinimumAccessLevelRequirement(AccessLevel.Viewer)));
               });

        return services;
    }

}