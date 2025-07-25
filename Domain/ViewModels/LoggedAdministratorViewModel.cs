
namespace TrackMyAssets_API.Domain.ViewModels;

public record LoggedAdministratorViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
}
