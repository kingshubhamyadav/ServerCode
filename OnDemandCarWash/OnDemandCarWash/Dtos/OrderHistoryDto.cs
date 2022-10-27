namespace OnDemandCarWash.Dtos
{
    public class OrderHistoryDto
    {
        public string timeOfWash { get; set; } = String.Empty;
        public string dateOfWash { get; set; } = String.Empty;
        public string location { get; set; } = String.Empty;

        //Payment details
        public string amountPaid { get; set; } = String.Empty;
        public string totalDiscount { get; set; } = String.Empty;
    }
}
