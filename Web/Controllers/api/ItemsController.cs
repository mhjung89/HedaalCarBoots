using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.api
{
    public class ItemsController : ApiBaseController
    {
        private readonly ITradeItemService _tradeItemService;

        public ItemsController(ITradeItemService tradeItemService)
        {
            _tradeItemService = tradeItemService;
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, ChangeStatusRq changeStatusRq)
        {
            var tradeItem = await _tradeItemService.GetByIdAsync(id, User);

            if (tradeItem == null)
            {
                return NotFound();
            }

            if (!tradeItem.IsOwner)
            {
                return BadRequest();
            }

            await _tradeItemService.ChangeStatusAsync(id, changeStatusRq.Status);

            return Ok();
        }
    }
}
