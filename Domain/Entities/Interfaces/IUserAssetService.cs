using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IUserAssetService
    {
        AssetTransaction AddUnits(Guid assetId, Guid userId, double units, string? note = null);
        AssetTransaction RemoveUnits(Guid assetId, Guid userId, double units, string? note = null);
        List<UserAsset> UserAssets(Guid userId);
        UserAsset? GetUserAssetByAssetId(Guid userId, Guid assetId);
        bool CheckData(Guid assetId, Guid userId);
    }
}