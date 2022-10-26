namespace OnDemandCarWash.Dtos
{
    public class SendInvoiceDto
    {
        public int orderId { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string timeOfWash { get; set; } = string.Empty;
        public string dateOfWash { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
    }
}
