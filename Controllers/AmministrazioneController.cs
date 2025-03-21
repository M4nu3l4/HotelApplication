using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelApplication.Models;

namespace HotelApplication.Controllers;


[Authorize(Roles = "Amministrativo")]
public class AmministrazioneController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

