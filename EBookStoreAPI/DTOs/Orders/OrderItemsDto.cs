namespace EBookStoreAPI.DTOs.Orders
{
    public class OrderItemsDto
    {
        public string OrderId { get; set; }
        public int BookId { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }
}
