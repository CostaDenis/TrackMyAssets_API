
namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IAssetService
    {
        void Create(Asset asset);
        Asset? GetById(Guid id);
        Asset? GetByName(string name);
        List<Asset> GetAll(int page = 0, int pageSize = 10);
        void Update(Asset asset);
        void Delete(Asset asset);
    }
}