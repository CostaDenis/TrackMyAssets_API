using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IAssetTransactionService
    {
        AssetTransaction? GetById(Guid id);
        AssetTransaction? GetByAssetId(Guid assetId);
        List<AssetTransaction>? GetAll(int? page = 1);
    }
}