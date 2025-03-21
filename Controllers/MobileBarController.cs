
using Microsoft.AspNetCore.Mvc;
using HotelApplication.Data;
using Microsoft.EntityFrameworkCore;
using HotelApplication.Models;

namespace HotelApplication.Controllers
{
    public class MobileBarController : Controller
    {
        private readonly HotelApplicationContext _context;

        public MobileBarController(HotelApplicationContext context)
        {
            _context = context;
        }

        // GET: MobileBar
        public async Task<IActionResult> Index()
        {
            return View(await _context.MobileBarItems.ToListAsync());
        }

        // GET: MobileBar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobileBarItem = await _context.MobileBarItems
                .FirstOrDefaultAsync(m => m.MobileBarItemId == id);
            if (mobileBarItem == null)
            {
                return NotFound();
            }

            return View(mobileBarItem);
        }

        // GET: MobileBar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MobileBar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MobileBarItemId,NomeProdotto,Prezzo")] MobileBarItem mobileBarItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mobileBarItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mobileBarItem);
        }

        // GET: MobileBar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobileBarItem = await _context.MobileBarItems.FindAsync(id);
            if (mobileBarItem == null)
            {
                return NotFound();
            }
            return View(mobileBarItem);
        }

        // POST: MobileBar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MobileBarItemId,NomeProdotto,Prezzo")] MobileBarItem mobileBarItem)
        {
            if (id != mobileBarItem.MobileBarItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mobileBarItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MobileBarItemExists(mobileBarItem.MobileBarItemId))
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
            return View(mobileBarItem);
        }

        // GET: MobileBar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobileBarItem = await _context.MobileBarItems
                .FirstOrDefaultAsync(m => m.MobileBarItemId == id);
            if (mobileBarItem == null)
            {
                return NotFound();
            }

            return View(mobileBarItem);
        }

        // POST: MobileBar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mobileBarItem = await _context.MobileBarItems.FindAsync(id);
            if (mobileBarItem != null)
            {
                _context.MobileBarItems.Remove(mobileBarItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MobileBarItemExists(int id)
        {
            return _context.MobileBarItems.Any(e => e.MobileBarItemId == id);
        }
    }
}
