using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.Entities
{
    public class AssetTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AssetId { get; set; }
        public Asset? Asset { get; set; }
        public double UnitsChanged { get; set; }  // Positivo para adições, negativo para remoções
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Type => UnitsChanged >= 0 ? "Addition" : "Removal";
        public string? Note { get; set; }
    }
}