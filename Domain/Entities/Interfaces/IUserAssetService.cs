namespace TrackMyAssets_API.Domain.Entities.Interfaces;

public interface IUserAssetService
{
    List<UserAsset> GetUserAssets(Guid userId);
    UserAsset? GetUserAssetByAssetId(Guid userId, Guid assetId);
    AssetTransaction AddUnits(Guid assetId, Guid userId, decimal units, string? note = null);
    AssetTransaction RemoveUnits(Guid assetId, Guid userId, decimal units, string? note = null);
    bool CheckData(Guid assetId, Guid userId);
    int CountUserAsset();
}
