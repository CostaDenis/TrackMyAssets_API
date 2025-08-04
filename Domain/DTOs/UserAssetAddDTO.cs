using System.ComponentModel.DataAnnotations;

namespace TrackMyAssets_API.Domain.DTOs;

public class UserAssetAddDTO
{
    public Guid AssetId { get; set; }

    [Range(typeof(decimal), "0.0001", "79228162514264337593543950335")]
    public decimal Units { get; set; }
    public string? Note { get; set; }
}
