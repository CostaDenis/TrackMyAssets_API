using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.Entities.Interfaces;

namespace TrackMyAssets_API.Domain.Entities.Services
{
    public class AssetService : IAssetService
    {

        public AssetService(AppDbContext context)
        {
            _context = context;
        }

        private readonly AppDbContext _context;

        public void Create(Asset asset)
        {
            _context.Assets.Add(asset);
            _context.SaveChanges();
        }

        public Asset? GetById(Guid id)
            => _context.Assets.Where(x => x.Id == id).FirstOrDefault();


        public List<Asset> GetAll(int? page = 1)
        {
            var query = _context.Assets.AsQueryable();
            int pageSize = 10;

            if (page != null)
                query = query.Skip(((int)page - 1) * pageSize).Take(pageSize);

            return query.ToList();
        }

        public void Update(Asset asset)
        {
            _context.Assets.Update(asset);
            _context.SaveChanges();
        }

        public void Delete(Asset asset)
        {
            _context.Assets.Remove(asset);
            _context.SaveChanges();
        }

        public AssetTransaction AddUnits(Guid assetId, double unit, string? note = null)
        {
            throw new NotImplementedException();
        }

        public AssetTransaction RemoveUnis(Guid assetId, double unit, string? note = null)
        {
            throw new NotImplementedException();
        }

    }
}