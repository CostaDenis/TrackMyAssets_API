using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IAssetService
    {
        void Create(Asset asset);
        Asset? GetById(Guid id);
        List<Asset> GetAll(int? page = 1, string? name = null, string? symbol = null);
        void Update(Asset asset);
        void Delete(Asset asset);
    }
}