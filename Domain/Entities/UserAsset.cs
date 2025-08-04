using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrackMyAssets_API.Domain.Entities;

public class UserAsset
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
    public Guid AssetId { get; set; }

    [JsonIgnore]
    public Asset? Asset { get; set; }
    public decimal Units { get; set; }
}
