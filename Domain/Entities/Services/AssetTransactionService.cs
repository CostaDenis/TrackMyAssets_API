using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.Entities.Interfaces;

namespace TrackMyAssets_API.Domain.Entities.Services;

public class AssetTransactionService : IAssetTransactionService
{
    private readonly AppDbContext _context;

    public AssetTransactionService(AppDbContext context)
    {
        _context = context;
    }

    public List<AssetTransaction>? GetAll(Guid userId, int page = 0, int pageSize = 10)
    {
        var query = _context.AssetTransactions
            .Where(x => x.UserId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsQueryable();

        return query.ToList();
    }

    public AssetTransaction? GetById(Guid assetTransactionId)
        => _context.AssetTransactions.Find(assetTransactionId);

    public AssetTransaction? GetByAssetId(Guid assetId)
        => _context.AssetTransactions.FirstOrDefault(x => x.AssetId == assetId);

}
