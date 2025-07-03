using System.Security.Claims;


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