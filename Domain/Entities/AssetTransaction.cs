using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrackMyAssets_API.Domain.Entities;

public class AssetTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AssetId { get; set; }

    [JsonIgnore]
    public Asset? Asset { get; set; }
    public Guid UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
    public decimal UnitsChanged { get; set; }  // Positivo para adições, negativo para remoções
    public DateTime Date { get; set; }
    public string Type { get; set; } = "Addition";
    public string? Note { get; set; }
}
