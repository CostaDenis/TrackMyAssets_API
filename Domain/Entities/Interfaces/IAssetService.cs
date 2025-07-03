
namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IAssetService
    {
        void Create(Asset asset);
        Asset? GetById(Guid id);
        List<Asset> GetAll(int? page = 1);
        void Update(Asset asset);
        void Delete(Asset asset);
    }
}