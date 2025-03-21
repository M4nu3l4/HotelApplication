namespace HotelApplication.Models
{
    public class Camera
    {
        public int CameraId { get; set; }
        public required  string Numero { get; set; }
        public string? Tipo { get; set; }
        public decimal Prezzo { get; set; }
        public bool Fumatori { get; set; }
        public bool Disabile { get; set; } // 10% di sconto
        public bool Balcone => Fumatori; // True se Fumatori
        public bool MezzaPensione { get; set; }
        public bool PensioneCompleta { get; set; }

        public ICollection<CameraMobileBar>? MobileBarItems { get; set; }
        public required ICollection<Prenotazione> Prenotazioni { get; set; }
    }
}
