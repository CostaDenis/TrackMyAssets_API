
namespace TrackMyAssets_API.Domain.ModelsViews
{
    public record LoggedUserModelView
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}