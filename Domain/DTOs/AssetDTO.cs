using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.DTOs
{
    public class AssetDTO
    {
        public string Name { get; set; } = default!;
        public string Symbol { get; set; } = default!;
        public string Type { get; set; } = default!;
        public double Units { get; set; }
    }
}