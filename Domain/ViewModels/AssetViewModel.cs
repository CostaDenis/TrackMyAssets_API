namespace TrackMyAssets_API.Domain.ViewModels;

public record AssetViewModel
{
    public string Name { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public string Type { get; set; } = default!;
}
