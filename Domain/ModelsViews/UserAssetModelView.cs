using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.ModelsViews
{
    public record UserAssetModelView
    {
        // public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AssetId { get; set; }
        public decimal Units { get; set; }
    }
}