using Pishgaman.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pishgaman.JwtHelper
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, ApplicationUser user, List<string> userRoles);
        //string GenerateJSONWebToken(string key, string issuer, ApplicationUser user);
        bool IsTokenValid(string key, string issuer, string token);
    }
}
