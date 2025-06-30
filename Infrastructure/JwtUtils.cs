using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace TrackMyAssets_API.Infrastructure
{
    public static class JwtUtils
    {

        public static Guid? GetUserId(this HttpContext http)
        {
            var claim = http.User.FindFirst(ClaimTypes.NameIdentifier);
            return Guid.TryParse(claim?.Value, out var id) ? id : null;
        }


    }
}