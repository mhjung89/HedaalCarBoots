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
            if (_context.TradeItems == null)
            {
                return Problem("Entity set 'HCBDbContext.TradeItems'  is null.");
            }

            var tradeItems = await _context.TradeItems.Select(x => new TradeItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Negotiable = x.Negotiable,
                IsOwner = x.SellerId == User.GetUserId()
            }).ToListAsync();

            return View(tradeItems);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TradeItems == null)
            {
                return NotFound();
            }

            var tradeItem = await _context.TradeItems.Select(x => new TradeItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Negotiable = x.Negotiable,
                IsOwner = x.SellerId == User.GetUserId()
            }).FirstOrDefaultAsync(m => m.Id == id);

            if (tradeItem == null)
            {
                return NotFound();
            }

            return View(tradeItem);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View(new TradeItemInputDto());
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TradeItemInputDto input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            TradeItem tradeItem = new TradeItem
            {
                Name = input.Name,
                Description = input.Description,
                Price = input.Price,
                Negotiable = input.Negotiable,
                SellerId = User.GetUserId()
            };

            _context.Add(tradeItem);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TradeItems == null)
            {
                return NotFound();
            }

            var tradeItem = await _context.TradeItems.Select(x => new TradeItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Negotiable = x.Negotiable,
                IsOwner = x.SellerId == User.GetUserId()
            }).FirstOrDefaultAsync(m => m.Id == id);

            if (tradeItem == null)
            {
                return NotFound();
            }

            if (!tradeItem.IsOwner)
            {
                return Forbid();
            }

            return View(tradeItem);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TradeItemInputDto input)
        {
            if (id != input.Id)
            {
                return NotFound();
            }

            var tradeItem = await _context.TradeItems.FindAsync(id);

            if (tradeItem == null)
            {
                return NotFound();
            }

            if (tradeItem.SellerId != User.GetUserId())
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            try
            {
                tradeItem.Name = input.Name;
                tradeItem.Description = input.Description;
                tradeItem.Price = input.Price;
                tradeItem.Negotiable = input.Negotiable;

                _context.Update(tradeItem);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeItemExists(input.Id))
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

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TradeItems == null)
            {
                return NotFound();
            }

            var tradeItem = await _context.TradeItems.Select(x => new TradeItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Negotiable = x.Negotiable,
                IsOwner = x.SellerId == User.GetUserId()
            }).FirstOrDefaultAsync(m => m.Id == id);

            if (tradeItem == null)
            {
                return NotFound();
            }

            if (!tradeItem.IsOwner)
            {
                return Forbid();
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

            if (tradeItem == null)
            {
                return NotFound();
            }

            if (tradeItem.SellerId != User.GetUserId())
            {
                return Forbid();
            }

            _context.TradeItems.Remove(tradeItem);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TradeItemExists(int id)
        {
            return (_context.TradeItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
