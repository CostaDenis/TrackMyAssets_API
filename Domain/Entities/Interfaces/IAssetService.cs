using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.Entities.Interfaces
{
    public interface IAssetService
    {
        void Create(Asset asset);  //Adm
        Asset? GetById(Guid id);  //User e Adm
        List<Asset> GetAll(int? page = 1);  //User e Adm
        void Update(Asset asset);  //Adm
        void Delete(Asset asset);  //Adm
        AssetTransaction AddUnits(Guid assetId, double unit, string? note = null);  //User
        AssetTransaction RemoveUnis(Guid assetId, double unit, string? note = null);  //User
    }
}