
namespace TrackMyAssets_API.Domain.ViewModels;

public record UserAssetViewModel
{
    public Guid UserId { get; set; }
    public Guid AssetId { get; set; }
    public decimal Units { get; set; }
}
