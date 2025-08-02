using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.Entities.Interfaces;

namespace TrackMyAssets_API.Domain.Entities.Services;

public class AssetService : IAssetService
{
    private readonly AppDbContext _context;

    public AssetService(AppDbContext context)
    {
        _context = context;
    }

    public List<Asset> GetAll(int page = 0, int pageSize = 10)
    {
        var query = _context.Assets.AsQueryable();
        query = query.Skip(((int)page - 1) * pageSize).Take(pageSize);

        return query.ToList();
    }

    public Asset? GetById(Guid id)
        => _context.Assets.Where(x => x.Id == id).FirstOrDefault();

    public Asset? GetByName(string name)
        => _context.Assets.Where(x => x.Name == name).FirstOrDefault();

    public void Create(Asset asset)
    {
        _context.Assets.Add(asset);
        _context.SaveChanges();
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

    public int CountAsset()
        => _context.Assets.Count();

}
