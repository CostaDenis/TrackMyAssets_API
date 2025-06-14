using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackMyAssets_API.Domain.Enums;

namespace TrackMyAssets_API.Domain.Entities
{
    public class Asset
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = default!;
        public string? Symbol { get; set; }
        public EAsset Type { get; set; }
        public double Units { get; set; }
    }
}