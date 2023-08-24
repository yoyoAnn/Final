namespace EBookStoreAPI.VM.OrderVM
{
    public class OrdersVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhone { get; set; }
        public string TaxIdNum { get; set; }
        public string VehicleNum { get; set; }
        public string Remark { get; set; }
        public DateTime OrderTime { get; set; }
        public string OrderStatusName { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingNumber { get; set; }
        public DateTime? ShippingTime { get; set; }
        public decimal ShippingFee { get; set; }
        public string ShippingStatusName { get; set; }
        public decimal TotalPayment { get; set; }
        public bool CommentStatus { get; set; }
    }
}
