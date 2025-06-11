using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}