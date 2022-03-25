using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace EduPlan.ChatApp.Api;

public static class ServiceCollectionExtensions
{
    public static AuthenticationBuilder AddJwtBearerCustom(this AuthenticationBuilder builder, string jwtSecret)
    {
        builder.AddJwtBearer(x =>
         {
             x.RequireHttpsMetadata = true;
             x.SaveToken = true;
             x.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidIssuer = Constants.JwtIssuer,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                 ValidAudience = Constants.JwtAudience,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ClockSkew = TimeSpan.FromMinutes(1)
             };
             x.Events = new JwtBearerEvents
             {
                 OnAuthenticationFailed = context =>
                 {
                     if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                     {
                         context.Response.Headers.Add("Token-Expired", "true");
                     }
                     return Task.CompletedTask;
                 }
             };
         });

        return builder;
    }
}
