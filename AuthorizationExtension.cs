using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MadSoul.AspCommon;

public static class AuthorizationExtension
{
    public static void ConfigureAuthorization(this  IServiceCollection services , IConfiguration  configuration )
    {
        var issuer = configuration["Authorization:Issuer"] ?? "default_issuer";
        var audience = configuration["Authorization:Audience"] ?? "default_audience";
        var  key =  configuration["Authorization:Key"] ?? "default_ket";
        var  tokenExpireMinutes =  configuration["Authorization:AccessTokenExpirationMinutes"] ?? "15";
        var  refreshTokenExpireDays =  configuration["Authorization:RefreshTokenExpirationDays"] ?? "7";
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
    }
    
    
    
    
    
}