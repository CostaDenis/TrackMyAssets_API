
namespace TrackMyAssets_API.Domain.ViewModels;

public record UserViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
}
