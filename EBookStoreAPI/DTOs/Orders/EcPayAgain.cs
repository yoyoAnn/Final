namespace EBookStoreAPI.DTOs.Orders
{
    public class EcPayAgain
    {
        public string orderId { get; set; }
        public string? name { get; set; }
        public int price { get; set; }
        public int qty { get; set; }
    }
}
