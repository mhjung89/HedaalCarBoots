using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ITradeItemService _tradeItemService;

        public ItemsController(ITradeItemService tradeItemService)
        {
            _tradeItemService = tradeItemService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<TradeItemDto> tradeItems = await _tradeItemService.GetAllAsync(User);

            return View(tradeItems);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tradeItem = await _tradeItemService.GetByIdAsync(id, User);

            if (tradeItem == null)
            {
                return NotFound();
            }

            return View(tradeItem);
        }

        public IActionResult Create()
        {
            return View(new TradeItemInputDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TradeItemInputDto input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            await _tradeItemService.CreateAsync(input, User);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            TradeItemDto? tradeItem = await _tradeItemService.GetByIdAsync(id, User);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TradeItemInputDto input)
        {
            if (id != input.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var tradeItem = await _tradeItemService.GetByIdAsync(id, User);

            if (tradeItem == null)
            {
                return NotFound();
            }

            if (!tradeItem.IsOwner)
            {
                return Forbid();
            }

            await _tradeItemService.UpdateAsync(input);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tradeItem = await _tradeItemService.GetByIdAsync(id, User);

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tradeItem = await _tradeItemService.GetByIdAsync(id, User);

            if (tradeItem == null)
            {
                return NotFound();
            }

            if (!tradeItem.IsOwner)
            {
                return Forbid();
            }

            await _tradeItemService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
