
namespace TrackMyAssets_API.Domain.ModelsViews
{
    public record AssetModelView
    {
        public string Name { get; set; } = default!;
        public string Symbol { get; set; } = default!;
        public string Type { get; set; } = default!;
    }
}