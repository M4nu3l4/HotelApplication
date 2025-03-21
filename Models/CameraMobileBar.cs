using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApplication.Models
{
    public class CameraMobileBar
    {
        [Key]
        [Column(Order = 1)]
        public int CameraId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int MobileBarItemId { get; set; }

        // Navigational properties
        public Camera Camera { get; set; }
        public MobileBarItem MobileBarItem { get; set; }
    }
}
