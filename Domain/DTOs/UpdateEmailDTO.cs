namespace TrackMyAssets_API.Domain.DTOs;

public class UpdateEmailDTO
{
    public string NewEmail { get; set; } = default!;
    public string NewEmailConfirmation { get; set; } = default!;
}