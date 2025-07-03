
namespace TrackMyAssets_API.Domain.ModelsViews
{
    public record LoggedAdministratorModelView
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}