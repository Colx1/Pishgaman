using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pishgaman.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pishgaman.JwtHelper
{
    public class TokenService : ITokenService
    {
        private const int EXPIRY_DURATION_DAYS = 1;

        private readonly UserManager<ApplicationUser> userManager;
        public TokenService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public string BuildToken(string key, string issuer, ApplicationUser user, List<string> userRoles)
        {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, user.UserName),
                //new Claim(ClaimTypes.Role, ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.AddDays(EXPIRY_DURATION_DAYS), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public bool IsTokenValid(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
