using Core.Types;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.TradeItems
{
    public class TradeItemDetailViewModel
    {
        public TradeItemDto TradeItem { get; set; } = null!;
        public SelectList TradeItemStatusOptions { get; set; } = new SelectList(Enum.GetValues(typeof(ETradeItemStatus)));
    }
}
