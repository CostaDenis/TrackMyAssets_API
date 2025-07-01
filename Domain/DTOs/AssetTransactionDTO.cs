using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.DTOs
{
    public class AssetTransactionDTO
    {
        public Guid AssetId { get; set; }
        public decimal UnitsChanged { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = default!;
        public string? Note { get; set; }
    }
}