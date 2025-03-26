
using System.ComponentModel.DataAnnotations;
  
namespace HotelApplication.Models
{
    public class Prenotazione
    {
        public int PrenotazioneId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int CameraId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataInizio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataFine { get; set; }

        [Display(Name = "Disponibile")]
        public bool StatoDisponibile { get; set; }


        [Required]
        public decimal ImportoTotale { get; set; }

        public bool Fumatore { get; set; }

        [Required]
        public required  string DipendenteId { get; set; }

        // Relazioni di navigazione
        public required Cliente Cliente { get; set; }
        public required Camera Camera { get; set; }
        public required ApplicationUser Dipendente { get; set; }
    }

}
