using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IAssetTransactionService
    {
        void GetById(Guid id);
        void GetByAssetId(Guid assetId);
        List<AssetTransaction> GetAll(int? page = 1);
    }
}