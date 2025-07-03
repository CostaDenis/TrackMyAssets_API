
namespace TrackMyAssets_API.Domain.DTOs
{
    public class AssetDTO
    {
        public string Name { get; set; } = default!;
        public string Symbol { get; set; } = default!;
        public string Type { get; set; } = default!;
    }
}