using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.DTOs
{
    public class UserAssetRemoveDTO
    {
        public Guid AssetId { get; set; }

        [Range(-0.0001, double.MinValue, ErrorMessage = "Quantidade inválida.")]
        public double Units { get; set; }

        public string? Note { get; set; }
    }
}