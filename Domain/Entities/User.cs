using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrackMyAssets_API.Domain.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(255, ErrorMessage = "O email não pode exceder 255 caracteres.")]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(256, ErrorMessage = "A senha não pode exceder 256 caracteres.")]
    public string Password { get; set; } = default!;

    [JsonIgnore]
    public List<UserAsset> UserAssets { get; set; } = new();
}
