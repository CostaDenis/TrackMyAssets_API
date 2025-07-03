using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TrackMyAssets_API.Domain.Enums;

namespace TrackMyAssets_API.Domain.Entities
{
    public class Asset
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(60, ErrorMessage = "O nome não pode exceder 60 caracteres.")]
        public string Name { get; set; } = default!;

        [StringLength(10, ErrorMessage = "O símbolo não pode exceder 10 caracteres.")]
        public string? Symbol { get; set; }

        [Required]
        public EAsset Type { get; set; }

        [JsonIgnore]
        public List<UserAsset> UserAssets { get; set; } = new();
    }
}