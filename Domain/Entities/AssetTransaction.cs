using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrackMyAssets_API.Domain.Entities;

public class AssetTransaction
{
    // [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // [Required]
    public Guid AssetId { get; set; }

    // [Required]
    [JsonIgnore]
    public Asset? Asset { get; set; }

    // [Required]
    public Guid UserId { get; set; }

    // [Required]
    [JsonIgnore]
    public User? User { get; set; }

    // [Required]
    public decimal UnitsChanged { get; set; }  // Positivo para adições, negativo para remoções
    public DateTime Date { get; set; }

    // [Required]
    // [MaxLength(20)]
    public string Type { get; set; } = "Addition";

    // [StringLength(500, ErrorMessage = "A observação não pode execder 500 caracteres.")]
    public string? Note { get; set; }
}
