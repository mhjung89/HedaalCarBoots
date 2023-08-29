using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class ItemsController : Controller
    {
        private readonly HCBDbContext _context;

        public ItemsController(HCBDbContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
              return _context.TradeItems != null ? 
                          View(await _context.TradeItems.ToListAsync()) :
                          Problem("Entity set 'HCBDbContext.TradeItems'  is null.");
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TradeItems == null)
            {
                return NotFound();
            }

            var tradeItem = await _context.TradeItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tradeItem == null)
            {
                return NotFound();
            }

            return View(tradeItem);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] TradeItem tradeItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tradeItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tradeItem);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TradeItems == null)
            {
                return NotFound();
            }

            var tradeItem = await _context.TradeItems.FindAsync(id);
            if (tradeItem == null)
            {
                return NotFound();
            }
            return View(tradeItem);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] TradeItem tradeItem)
        {
            if (id != tradeItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tradeItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TradeItemExists(tradeItem.Id))
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
            return View(tradeItem);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TradeItems == null)
            {
                return NotFound();
            }

            var tradeItem = await _context.TradeItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tradeItem == null)
            {
                return NotFound();
            }

            return View(tradeItem);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TradeItems == null)
            {
                return Problem("Entity set 'HCBDbContext.TradeItems'  is null.");
            }
            var tradeItem = await _context.TradeItems.FindAsync(id);
            if (tradeItem != null)
            {
                _context.TradeItems.Remove(tradeItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TradeItemExists(int id)
        {
          return (_context.TradeItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
