namespace TrackMyAssets_API.Domain.ViewModels;

public record AssetTransactionViewModel
{
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }
    public decimal UnitsChanged { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = default!;
    public string? Note { get; set; }
}