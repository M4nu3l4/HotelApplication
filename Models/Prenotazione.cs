
using System.ComponentModel.DataAnnotations;

namespace HotelApplication.Models
{
    public class Prenotazione
    {
        [Key]
        public int PrenotazioneId { get; set; }
        public int ClienteId { get; set; }
        public required Cliente Cliente { get; set; }

        public int CameraId { get; set; }
        public required Camera Camera { get; set; }

        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public string? Stato { get; set; }
        public bool Fumatore { get; set; }
        public decimal ImportoTotale { get; set; }
        public required string DipendenteId { get; set; }
        public required ApplicationUser Dipendente { get; set; }

        public required ICollection<PrenotazioneMobileBar> MobileBarItems { get; set; }
    }
}
