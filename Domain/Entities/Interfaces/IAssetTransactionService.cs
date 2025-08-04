namespace TrackMyAssets_API.Domain.Entities.Interfaces;

public interface IAssetTransactionService
{
    List<AssetTransaction>? GetAll(Guid userId, int page = 0, int pageSize = 10);
    AssetTransaction? GetById(Guid id);
    AssetTransaction? GetByAssetId(Guid assetId);
}
