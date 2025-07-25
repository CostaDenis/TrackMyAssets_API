namespace TrackMyAssets_API.Domain.DTOs;

public class UpdatePasswordDTO
{
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string NewPasswordConfirmation { get; set; } = default!;
}