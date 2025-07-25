
namespace TrackMyAssets_API.Domain.ModelsViews
{
    public record UserModelView
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
    }
}