namespace TrackMyAssets_API.Domain.Entities.Interfaces;

public interface ITokenService
{
    Guid? GetUserId(HttpContext http);
    string GenerateTokenJwt(Guid id, string email, string role);
}