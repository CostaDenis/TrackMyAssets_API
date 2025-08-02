namespace TrackMyAssets_API.Domain.Entities.Interfaces;

public interface IAssetService
{
    List<Asset> GetAll(int page = 0, int pageSize = 10);
    Asset? GetById(Guid id);
    Asset? GetByName(string name);
    void Create(Asset asset);
    void Update(Asset asset);
    void Delete(Asset asset);
    int CountAsset();
}
