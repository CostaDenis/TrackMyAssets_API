using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrackMyAssets_API.Data;
using TrackMyAssets_API.Domain.Entities.Interfaces;

namespace TrackMyAssets_API.Domain.Entities.Services
{
    public class UserAssetService : IUserAssetService
    {

        public UserAssetService(AppDbContext context)
        {
            _context = context;
        }

        private readonly AppDbContext _context;

        public AssetTransaction AddUnits(Guid assetId, Guid userId, double units, string? note = null)
        {

            if (units <= 0)
                throw new ArgumentException("As unidades devem ser positivas!");

            if (CheckData(assetId, userId) == false)
                throw new ArgumentException("Erro. Confira os IDs!");


            var asset = _context.Assets.Find(assetId)!;
            var user = _context.Users.Find(userId)!;
            var AssetTransaction = new AssetTransaction
            {
                AssetId = assetId,
                Asset = asset,
                UserId = userId,
                User = user,
                UnitsChanged = units,
                Note = note
            };

            var userAsset = new UserAsset
            {
                UserId = userId,
                AssetId = assetId,
                Units = units
            };

            _context.UserAssets.Add(userAsset);
            _context.SaveChanges();

            return AssetTransaction;
        }

        public AssetTransaction RemoveUnits(Guid assetId, Guid userId, double units, string? note = null)
        {
            if (units >= 0)
                throw new ArgumentException("As unidades devem ser negativas!");

            if (CheckData(assetId, userId) == false)
                throw new ArgumentException("Erro. Confira os IDs!");


            var asset = _context.Assets.Find(assetId)!;
            var user = _context.Users.Find(userId)!;
            var AssetTransaction = new AssetTransaction
            {
                AssetId = assetId,
                Asset = asset,
                UserId = userId,
                User = user,
                UnitsChanged = units,
                Note = note
            };

            var userAsset = new UserAsset
            {
                UserId = userId,
                AssetId = assetId,
                Units = units
            };

            _context.UserAssets.Add(userAsset);
            _context.SaveChanges();

            return AssetTransaction;
        }

        public List<UserAsset> UserAssets(Guid userId)
            => _context.UserAssets
                .Where(x => x.UserId == userId)
                .ToList();

        public UserAsset? GetUserAssetByAssetId(Guid userId, Guid assetId)
            => _context.UserAssets
                .FirstOrDefault(x => x.UserId == userId &&
                x.AssetId == assetId);

        public bool CheckData(Guid assetId, Guid userId)
        {
            var asset = _context.Assets.Find(assetId);
            var user = _context.Users.Find(userId);

            if (asset == null || user == null)
            {
                return false;
            }

            return true;
        }
    }
}