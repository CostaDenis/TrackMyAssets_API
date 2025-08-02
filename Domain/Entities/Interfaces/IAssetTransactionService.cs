namespace TrackMyAssets_API.Domain.Entities.Interfaces;

public interface IAssetTransactionService
{
    List<AssetTransaction>? GetAll(int? page = 1);
    AssetTransaction? GetById(Guid id);
    AssetTransaction? GetByAssetId(Guid assetId);
}
