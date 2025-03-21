namespace HotelApplication.Models
{
    public class MobileBarItem
    {
        public int MobileBarItemId { get; set; }
        public required string NomeProdotto { get; set; }
        public decimal Prezzo { get; set; }

        public required ICollection<CameraMobileBar> Camere { get; set; }
        public required ICollection<PrenotazioneMobileBar> Prenotazioni { get; set; }
    }
}
