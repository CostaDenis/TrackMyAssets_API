using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Exceptions;

namespace TrackMyAssets_API.Domain.Entities.Services;

public class UserAssetService : IUserAssetService
{

    private readonly AppDbContext _context;

    public UserAssetService(AppDbContext context)
    {
        _context = context;
    }

    public List<UserAsset> GetUserAssets(Guid userId)
        => _context.UserAssets
            .Where(x => x.UserId == userId)
            .ToList();

    public UserAsset? GetUserAssetByAssetId(Guid userId, Guid assetId)
        => _context.UserAssets
            .FirstOrDefault(x => x.UserId == userId &&
            x.AssetId == assetId);

    public AssetTransaction AddTransaction(Guid assetId, Guid userId, decimal units, string? note = null)
    {
        if (units == 0)
            throw new UnitsZeroException("As unidades não podem ser zero!");

        if (units > Decimal.MaxValue)
            throw new UnitsMaxException("As unidades não podem ser maior que '79,228,162,514,264,337,593,543,950,335'!");

        if (CheckData(assetId, userId) == false)
            throw new InvalidIdException("Erro. Confira os IDs!");

        var currentUnits = GetAssetAmount(assetId, userId);

        if (units < 0 && currentUnits + units < 0)
            throw new InsufficientBalance("Saldo insuficiente para remover essa quantidade de unidades.");

        var asset = _context.Assets.Find(assetId)!;
        var user = _context.Users.Find(userId)!;

        var assetTransaction = new AssetTransaction
        {
            AssetId = assetId,
            Asset = asset,
            UserId = userId,
            User = user,
            UnitsChanged = units,
            Note = note
        };

        _context.AssetTransactions.Add(assetTransaction);
        _context.SaveChanges();

        return assetTransaction;
    }

    public bool CheckData(Guid assetId, Guid userId)
    {
        var asset = _context.Assets.Find(assetId);
        var user = _context.Users.Find(userId);

        if (asset == null || user == null)
            return false;

        return true;
    }

    public decimal GetAssetAmount(Guid assetId, Guid userId)
        => _context.AssetTransactions
                .Where(t => t.UserId == userId && t.AssetId == assetId)
                .Sum(t => t.UnitsChanged);


    public int CountUserAsset()
        => _context.UserAssets.Count();

}
