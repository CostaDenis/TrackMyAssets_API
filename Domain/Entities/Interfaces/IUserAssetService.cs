
namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IUserAssetService
    {
        AssetTransaction AddUnits(Guid assetId, Guid userId, decimal units, string? note = null);
        AssetTransaction RemoveUnits(Guid assetId, Guid userId, decimal units, string? note = null);
        List<UserAsset> UserAssets(Guid userId);
        UserAsset? GetUserAssetByAssetId(Guid userId, Guid assetId);
        bool CheckData(Guid assetId, Guid userId);
    }
}