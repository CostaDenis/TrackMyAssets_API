namespace TrackMyAssets_API.Domain.ViewModels;

public record DashBoardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalAssets { get; set; }
    public int TotalUserAssets { get; set; }
}
