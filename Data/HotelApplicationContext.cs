using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelApplication.Models;

namespace HotelApplication.Data
{
    public class HotelApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public HotelApplicationContext(DbContextOptions<HotelApplicationContext> options) : base(options) { }

        public DbSet<Cliente> Clienti { get; set; }
        public DbSet<Camera> Camere { get; set; }
        public DbSet<Prenotazione> Prenotazioni { get; set; }
        public DbSet<MobileBarItem> MobileBarItems { get; set; }
        public DbSet<CameraMobileBar> CameraMobileBars { get; set; }
        public DbSet<PrenotazioneMobileBar> PrenotazioneMobileBars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Camera>().Property(c => c.Prezzo).HasPrecision(10, 2);
            modelBuilder.Entity<MobileBarItem>().Property(mb => mb.Prezzo).HasPrecision(10, 2);
            modelBuilder.Entity<Prenotazione>().Property(p => p.ImportoTotale).HasPrecision(10, 2);

            modelBuilder.Entity<CameraMobileBar>()
                .HasKey(cmb => new { cmb.CameraId, cmb.MobileBarItemId });

            modelBuilder.Entity<PrenotazioneMobileBar>()
                .HasKey(pmb => new { pmb.PrenotazioneId, pmb.MobileBarItemId });
        }
    }
}

