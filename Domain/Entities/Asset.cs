using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TrackMyAssets_API.Domain.Enums;

namespace TrackMyAssets_API.Domain.Entities;

public class Asset
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string? Symbol { get; set; }
    public EAsset Type { get; set; }

    [JsonIgnore]
    public List<UserAsset> UserAssets { get; set; } = new();
}
