namespace Application.TradeItems
{
    public class TradeItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Negotiable { get; set; }
        public bool IsOwner { get; set; }
    }
}
