using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMyAssets_API.Domain.Entities
{
    public class Administrator
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255, ErrorMessage = "O email não pode exceder 255 caracteres.")]
        public string Email { get; set; } = default!;

        [Required]
        [StringLength(256, ErrorMessage = "A senha não pode exceder 256 caracteres.")]
        public string Password { get; set; } = default!;
    }
}