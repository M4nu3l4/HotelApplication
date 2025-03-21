using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelApplication.Data;
using HotelApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Authorize(Roles = "Amministrativo")]
public class AmministrazioneController : Controller
{
    private readonly HotelApplicationContext _context;

    public AmministrazioneController(HotelApplicationContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var prenotazioni = await _context.Prenotazioni
            .Include(p => p.Cliente)
            .Include(p => p.Dipendente)
            .Include(p => p.Camera)
            .ToListAsync();

      
        return View("Index", prenotazioni);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

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



}


