using System.ComponentModel.DataAnnotations;

namespace TrackMyAssets_API.Domain.DTOs
{
    public class UserAssetAddDTO
    {
        public Guid AssetId { get; set; }

        [Range(0.0001, (double)decimal.MaxValue, ErrorMessage = "Quantidade inválida.")]
        public decimal Units { get; set; }

        public string? Note { get; set; }
    }
}