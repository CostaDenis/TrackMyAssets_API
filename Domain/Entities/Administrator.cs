using System.ComponentModel.DataAnnotations;

namespace TrackMyAssets_API.Domain.Entities;

public class Administrator
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
