using Core.Entities;
using Core.Interfaces;
using System.Security.Claims;

namespace Application.TradeItems
{
    internal class TradeItemService : ITradeItemService
    {
        private readonly IRepository<TradeItem> _repository;

        public TradeItemService(IRepository<TradeItem> repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(TradeItemInputDto input, ClaimsPrincipal user)
        {
            var tradeItem = new TradeItem
            {
                Name = input.Name,
                Description = input.Description,
                Price = input.Price,
                Negotiable = input.Negotiable,
                SellerId = user.GetUserId()
            };

            _repository.Add(tradeItem);

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);

            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TradeItemDto>> GetAllAsync(ClaimsPrincipal user)
        {
            IEnumerable<TradeItem> tradeItems = await _repository.GetAllAsync();

            return tradeItems.Select(x => new TradeItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Negotiable = x.Negotiable,
                IsOwner = x.SellerId == user.GetUserId()
            });
        }

        public async Task<TradeItemDto?> GetByIdAsync(int id, ClaimsPrincipal user)
        {
            TradeItem? tradeItem = await _repository.GetByIdAsync(id);

            if (tradeItem == null)
            {
                return null;
            }

            return new TradeItemDto
            {
                Id = tradeItem.Id,
                Name = tradeItem.Name,
                Description = tradeItem.Description,
                Price = tradeItem.Price,
                Negotiable = tradeItem.Negotiable,
                IsOwner = tradeItem.SellerId == user.GetUserId()
            };
        }

        public async Task UpdateAsync(TradeItemInputDto input)
        {
            TradeItem? tradeItem = await _repository.GetByIdAsync(input.Id);

            if (tradeItem == null)
            {
                throw new ArgumentException("Trade item not found.");
            }

            tradeItem.Name = input.Name;
            tradeItem.Description = input.Description;
            tradeItem.Price = input.Price;
            tradeItem.Negotiable = input.Negotiable;

            _repository.Update(tradeItem);

            await _repository.SaveChangesAsync();
        }
    }
}
