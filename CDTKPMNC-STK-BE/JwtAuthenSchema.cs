using CDTKPMNC_STK_BE.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CDTKPMNC_STK_BE
{
    public class JwtAuthenSchema
    {
        public readonly IConfiguration _configuration;
        public SymmetricSecurityKey SecretKey { get; set; }
        public JwtAuthenSchema(IConfiguration configuration)
        {
            _configuration = configuration;
            var secretKey = _configuration.GetValue<string>("SecretKey") ?? throw new InvalidOperationException("Secret Key not found.");
            SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }

        public Action<JwtBearerOptions> Create(TokenType tokenType, UserType userType, bool validateLifetime)
        {
            return new Action<JwtBearerOptions>(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecretKey,
                    ValidateIssuer = true,
                    ValidIssuer = "Thương - Khôi - Sơn",
                    ValidateAudience = true,
                    ValidAudience = userType.ToString(),
                    ValidateLifetime = validateLifetime,
                    ClockSkew = TimeSpan.Zero,
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var tkType = context.Principal!.FindFirst("TokenType")?.Value;
                        if (tkType == null || tkType != tokenType.ToString())
                        {
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
