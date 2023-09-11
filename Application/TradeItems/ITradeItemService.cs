using Core.Types;
using System.Security.Claims;

namespace Application.TradeItems
{
    public interface ITradeItemService
    {
        Task ChangeStatusAsync(int id, ETradeItemStatus reserved);
        Task CreateAsync(TradeItemInputDto input, ClaimsPrincipal user);
        Task DeleteAsync(int id);
        Task<IEnumerable<TradeItemDto>> GetAllAsync(ClaimsPrincipal user);
        Task<TradeItemDto?> GetByIdAsync(int id, ClaimsPrincipal user);
        Task UpdateAsync(TradeItemInputDto input);
    }
}
