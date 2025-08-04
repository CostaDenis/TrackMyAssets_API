using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrackMyAssets_API.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    [JsonIgnore]
    public List<UserAsset> UserAssets { get; set; } = new();
}
