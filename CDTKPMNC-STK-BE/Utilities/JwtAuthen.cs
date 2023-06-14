using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CDTKPMNC_STK_BE.Utilities
{
    public enum TokenType
    {
        Access,
        Refresh
    }

    public class JwtAuthen
    {
        private readonly IConfiguration _configuration;
        public SymmetricSecurityKey SecretKey { get; set; }
        public JwtAuthen(IConfiguration configuration)
        {
            _configuration = configuration;
            var secretKey = _configuration.GetValue<string>("SecretKey") ?? throw new InvalidOperationException("Secret Key not found.");
            SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }
        
        /// <summary>
        /// Tạo JWT token với secret key tùy chọn
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public string GenerateJwtToken(Guid userId, AccountType userType, TokenType tokenType, int lifetimeMinutes)
        {
            var claims = new[]
                {
                    new Claim("TokenType", tokenType.ToString()),
                    new Claim("UserId", userId.ToString()),
                };
            var credentials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "Thương - Khôi - Sơn",
                audience: userType.ToString(),
                claims: claims,
                expires: DateTime.Now.AddMinutes(lifetimeMinutes),
                signingCredentials: credentials
            );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
        public AccountToken GenerateUserToken(Guid userId, AccountType userType)
        {
            string accessToken = GenerateAccessToken(userId, userType);
            string refreshToken = GenerateRefreshToken(userId, userType);
            return new AccountToken {AccessToken = accessToken, RefreshToken = refreshToken };
        }
        public string GenerateAccessToken(Guid userId, AccountType userType)
        {
            return GenerateJwtToken(userId, userType, TokenType.Access, 30);
        }

        public string GenerateRefreshToken(Guid userId, AccountType userType)
        {
            return GenerateJwtToken(userId, userType, TokenType.Refresh, 10 * 1440);
        }

        public Guid? VerifyJwtToken(string jwtToken, AccountType userType, TokenType tokenType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SecretKey,
                ValidateIssuer = true,
                ValidIssuer = "Thương - Khôi - Sơn",
                ValidateAudience = true,
                ValidAudience = userType.ToString(),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
                // var claims = tokenHandler.ReadJwtToken(jwtToken).Claims;
                // var userIdClaim = claims.FirstOrDefault(c => c.Type == "userId");
                var claims = claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value);
                if (claims["TokenType"] == tokenType.ToString())
                {
                    return new Guid(claims["UserId"]);
                }
                return null;
            }
            catch (Exception) 
            {

                return null; 
            }
        }

        public Guid? VerifyAccessToken(string jwtToken, AccountType userType)
        {
            return VerifyJwtToken(jwtToken, userType, TokenType.Access);
        }

        public Guid? VerifyRefreshToken(string jwtToken, AccountType userType)
        {
            return VerifyJwtToken(jwtToken, userType, TokenType.Refresh);
        }

        public Action<JwtBearerOptions> CreateAuthenSchema(TokenType tokenType, bool validateLifetime, params AccountType[] userTypes)
        {
            return new Action<JwtBearerOptions>(options =>
            {
                var validUserTypes = new string[userTypes.Length];
                for(int i = 0; i < validUserTypes.Length; i++)
                {
                    validUserTypes[i] = userTypes[i].ToString();
                }    

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecretKey,
                    ValidateIssuer = true,
                    ValidIssuer = "Thương - Khôi - Sơn",
                    ValidateAudience = true,
                    ValidAudiences = validUserTypes,
                    ValidateLifetime = validateLifetime,
                    ClockSkew = TimeSpan.Zero,
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var tkType = context.Principal!.FindFirst("TokenType")?.Value;
                        var userID = context.Principal!.FindFirst("UserId")?.Value;
                        var userType = context.Principal!.FindFirst("aud")?.Value;
                        if (tkType == null || tkType != tokenType.ToString() || userID == null)
                        {
                            context.Fail("Unauthorized");
                        }
                        context.HttpContext.Items["UserId"] = userID;
                        context.HttpContext.Items["AccountType"] = userType;
                        return Task.CompletedTask;
                    }
                };
            });
        }

    }
}
