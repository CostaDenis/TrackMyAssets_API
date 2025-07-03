using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.Entities.Interfaces;

namespace TrackMyAssets_API.Domain.Entities.Services
{
    public class AssetTransactionService : IAssetTransactionService
    {

        public AssetTransactionService(AppDbContext context)
        {
            _context = context;
        }

        private readonly AppDbContext _context;


        public List<AssetTransaction>? GetAll(int? page = 1)
        {
            var query = _context.AssetTransactions.AsQueryable();
            int pageSize = 10;

            if (page != null)
                query = query.Skip(((int)page - 1) * pageSize).Take(pageSize);

            return query.ToList();
        }

        public AssetTransaction? GetByAssetId(Guid assetId)
            => _context.AssetTransactions.FirstOrDefault(x => x.AssetId == assetId);

        public AssetTransaction? GetById(Guid assetTransactionId)
            => _context.AssetTransactions.Find(assetTransactionId);
    }
}