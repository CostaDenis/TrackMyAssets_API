using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.Entities.DTOs
{
    public class LoginDTO
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}