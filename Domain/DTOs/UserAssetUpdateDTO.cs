using System.ComponentModel.DataAnnotations;

namespace TrackMyAssets_API.Domain.DTOs;

public class UserAssetUpdateDTO
{
    public Guid AssetId { get; set; }

    public decimal Units { get; set; }

    public string? Note { get; set; }
}
