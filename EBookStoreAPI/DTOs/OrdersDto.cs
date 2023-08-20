namespace EBookStoreAPI.DTOs
{
    public class OrdersDto
    {
        public string? Id { get; set; }
        public int? UserId { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverPhone { get; set; }
        public string? VehicleNum { get; set; }
        public string? Remark { get; set; }
        public DateTime? OrderTime { get; set; }
        public int? OrderStatusId { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? ShippingFee { get; set; }
        public int? ShippingStatusId { get; set; }
        public decimal? TotalPayment { get; set; }
    }
}
