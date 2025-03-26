using HotelApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace HotelApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Credenziali errate.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Amministrativo"))
                    return RedirectToAction("Index", "Prenotazioni");
                else if (roles.Contains("AddettoPrenotazioni"))
                    return RedirectToAction("Index", "Prenotazioni");
                else if (roles.Contains("Admin"))
                    return RedirectToAction("Dashboard", "Account");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Credenziali errate.");
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaUtenti()
        {
            var utenti = await _userManager.Users.ToListAsync();
            return View(utenti);
        }

        [HttpGet]
        public async Task<IActionResult> GestisciRuoli(string id)
        {
            var utente = await _userManager.FindByIdAsync(id);
            if (utente == null) return NotFound();

            var ruoliUtente = await _userManager.GetRolesAsync(utente);
            var tuttiIRuoli = _roleManager.Roles.Select(r => r.Name).ToList();

            ViewBag.UserId = id;
            ViewBag.UserEmail = utente.Email;
            ViewBag.AllRoles = tuttiIRuoli;
            ViewBag.UserRoles = ruoliUtente;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GestisciRuoli(string id, List<string> ruoliSelezionati)
        {
            var utente = await _userManager.FindByIdAsync(id);
            if (utente == null) return NotFound();

            var ruoliAttuali = await _userManager.GetRolesAsync(utente);
            var risultatoRimozione = await _userManager.RemoveFromRolesAsync(utente, ruoliAttuali);
            if (!risultatoRimozione.Succeeded) return BadRequest("Errore durante la rimozione dei ruoli.");

            var risultatoAggiunta = await _userManager.AddToRolesAsync(utente, ruoliSelezionati);
            if (!risultatoAggiunta.Succeeded) return BadRequest("Errore durante l'aggiunta dei nuovi ruoli.");

            return RedirectToAction("ListaUtenti");
        }
    }
}
