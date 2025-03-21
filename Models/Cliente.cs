
using System.Collections.Generic;

namespace HotelApplication.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool Fumatore { get; set; }
        public bool Disabile { get; set; } // 10% di sconto
        public int NumeroBambini { get; set; } // Bambini 0-12 anni, 10% di sconto ciascuno

        public required ICollection<Prenotazione> Prenotazioni { get; set; }
    }
}
