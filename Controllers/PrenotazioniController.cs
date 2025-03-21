using System;
using HotelApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelApplication.Controllers
{

    [Authorize(Roles = "AddettoPrenotazioni")]
    public class PrenotazioniController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var hotelApplicationContext = _context.Prenotazioni.Include(p => p.Camera).Include(p => p.Cliente).Include(p => p.Dipendente);
            return View(await hotelApplicationContext.ToListAsync());
        }
        private readonly HotelApplicationContext _context;

        public PrenotazioniController(HotelApplicationContext context)
        {
            _context = context;
        }

     
        
        // GET: Prenotazioni/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prenotazione = await _context.Prenotazioni
                .Include(p => p.Camera)
                .Include(p => p.Cliente)
                .Include(p => p.Dipendente)
                .FirstOrDefaultAsync(m => m.PrenotazioneId == id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            return View(prenotazione);
        }

        // GET: Prenotazioni/Create
        public IActionResult Create()
        {
            ViewData["CameraId"] = new SelectList(_context.Camere, "CameraId", "CameraId");
            ViewData["ClienteId"] = new SelectList(_context.Clienti, "ClienteId", "ClienteId");
            ViewData["DipendenteId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Prenotazioni/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prenotazione prenotazione)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Stampa gli errori nella console di Visual Studio
                }
                return View(prenotazione);
            }

            try
            {
                _context.Prenotazioni.Add(prenotazione);
                await _context.SaveChangesAsync(); // Salva i dati nel database
                return RedirectToAction("Index"); // Torna alla lista delle prenotazioni
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il salvataggio: {ex.Message}");
                return View(prenotazione);
            }
        }

        // GET: Prenotazioni/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            ViewData["CameraId"] = new SelectList(_context.Camere, "CameraId", "CameraId", prenotazione.CameraId);
            ViewData["ClienteId"] = new SelectList(_context.Clienti, "ClienteId", "ClienteId", prenotazione.ClienteId);
            ViewData["DipendenteId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", prenotazione.DipendenteId);
            return View(prenotazione);
        }

        // POST: Prenotazioni/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrenotazioneId,ClienteId,CameraId,DataInizio,DataFine,Stato,Fumatore,ImportoTotale,DipendenteId")] Prenotazione prenotazione)
        {
            if (id != prenotazione.PrenotazioneId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prenotazione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrenotazioneExists(prenotazione.PrenotazioneId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CameraId"] = new SelectList(_context.Camere, "CameraId", "CameraId", prenotazione.CameraId);
            ViewData["ClienteId"] = new SelectList(_context.Clienti, "ClienteId", "ClienteId", prenotazione.ClienteId);
            ViewData["DipendenteId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", prenotazione.DipendenteId);
            return View(prenotazione);
        }

        // GET: Prenotazioni/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prenotazione = await _context.Prenotazioni
                .Include(p => p.Camera)
                .Include(p => p.Cliente)
                .Include(p => p.Dipendente)
                .FirstOrDefaultAsync(m => m.PrenotazioneId == id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            return View(prenotazione);
        }

        // POST: Prenotazioni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione != null)
            {
                _context.Prenotazioni.Remove(prenotazione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrenotazioneExists(int id)
        {
            return _context.Prenotazioni.Any(e => e.PrenotazioneId == id);
        }
    }
}
