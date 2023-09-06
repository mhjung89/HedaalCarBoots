using Core.Authorization;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize(Roles = HCBRoles.Basic)]
    public class ItemsController : ApiControllerBase
    {
        private readonly HCBDbContext _context;

        public ItemsController(HCBDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
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

            return Ok(tradeItems);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (_context.TradeItems == null)
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

            return Ok(tradeItem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TradeItemInputDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            return CreatedAtAction(nameof(GetById), new { id = tradeItem.Id }, tradeItem);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TradeItemInputDto input)
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
                return BadRequest(ModelState);
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

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
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

            return NoContent();
        }

        private bool TradeItemExists(int id)
        {
            return (_context.TradeItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
