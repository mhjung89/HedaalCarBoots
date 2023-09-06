using Core.Authentication;
using Core.Types;

namespace Core.Entities
{
    public class TradeItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Negotiable { get; set; }
        public ETradeItemStatus Status { get; set; }
        public DateTime? SoldAt { get; set; }
        public int SellerId { get; set; }
        public int? BuyerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ApplicationUser Seller { get; set; } = default!;
        public ApplicationUser? Buyer { get; set; }
    }
}
