using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string Cognome { get; set; } = string.Empty;

        [Required]
        public DateTime DataRegistrazione { get; set; } = DateTime.UtcNow;
    }
}
