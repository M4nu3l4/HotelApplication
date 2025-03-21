using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApplication.Models
{
    public class PrenotazioneMobileBar
    {
        [Key]
        [Column(Order = 1)]
        public int PrenotazioneId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int MobileBarItemId { get; set; }

        
        public required Prenotazione Prenotazione { get; set; }
        public required MobileBarItem MobileBarItem { get; set; }
    }
}
