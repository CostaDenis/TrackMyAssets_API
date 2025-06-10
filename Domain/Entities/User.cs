using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}