namespace TrackMyAssets_API.Domain.ViewModels;

public record HealthViewModel
{
    public string Status { get; set; } = default!;
    public DateTime Time { get; set; }
}
