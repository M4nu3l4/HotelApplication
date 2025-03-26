using HotelApplication.Data;
using HotelApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelApplication.Controllers
{
    [Authorize(Roles = "AddettoPrenotazioni, Amministrativo")]
    public class PrenotazioniController : Controller
    {
        private readonly HotelApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PrenotazioniController(HotelApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var prenotazioni = await _context.Prenotazioni
                .Include(p => p.Cliente)
                .Include(p => p.Camera)
                .Include(p => p.Dipendente)
                .ToListAsync();

            ViewBag.ImportoTotale = prenotazioni.Sum(p =>
            {
                decimal importo = p.ImportoTotale;
                if (p.Cliente.Disabile)
                    importo *= 0.9m; // sconto 10%
                return importo;
            });

            ViewBag.CommissioneDipendente = prenotazioni.Sum(p => p.ImportoTotale * 0.10m);

            return View(prenotazioni);
        }

        [Authorize(Roles = "AddettoPrenotazioni")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione == null) return NotFound();

            ViewData["ClienteId"] = new SelectList(_context.Clienti.Select(c => new
            {
                ClienteId = c.ClienteId,
                NomeCompleto = c.Nome + " " + c.Cognome
            }), "ClienteId", "NomeCompleto", prenotazione.ClienteId);

            ViewData["CameraId"] = new SelectList(_context.Camere.Select(c => new
            {
                CameraId = c.CameraId,
                Descrizione = c.Numero + " - " + c.Tipo
            }), "CameraId", "Descrizione", prenotazione.CameraId);

            ViewData["DipendenteId"] = new SelectList(_userManager.Users, "Id", "Email", prenotazione.DipendenteId);

            return View(prenotazione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prenotazione prenotazione)
        {
            if (id != prenotazione.PrenotazioneId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["ClienteId"] = new SelectList(_context.Clienti, "ClienteId", "Cognome", prenotazione.ClienteId);
                ViewData["CameraId"] = new SelectList(_context.Camere, "CameraId", "Numero", prenotazione.CameraId);
                ViewData["DipendenteId"] = new SelectList(_userManager.Users, "Id", "Email", prenotazione.DipendenteId);
                return View(prenotazione);
            }

            try
            {
                _context.Update(prenotazione);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Prenotazioni.Any(p => p.PrenotazioneId == prenotazione.PrenotazioneId))
                    return NotFound();
                else throw;
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "AddettoPrenotazioni")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var prenotazione = await _context.Prenotazioni
                .Include(p => p.Cliente)
                .Include(p => p.Camera)
                .FirstOrDefaultAsync(m => m.PrenotazioneId == id);

            if (prenotazione == null) return NotFound();

            return View(prenotazione);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione != null)
            {
                _context.Prenotazioni.Remove(prenotazione);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Blocco la creazione delle prenotazioni per l'amministrativo
        [HttpGet]
        public IActionResult Create()
        {
            return Forbid();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Prenotazione prenotazione)
        {
            return Forbid();
        }
    }
}