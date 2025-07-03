using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrackMyAssets_API.Domain.Entities
{
    public class UserAsset
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [JsonIgnore]
        public User? User { get; set; }

        [Required]
        public Guid AssetId { get; set; }

        [Required]
        [JsonIgnore]
        public Asset? Asset { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal Units { get; set; }
    }
}