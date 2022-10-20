namespace OnDemandCarWash.Dtos
{
    public class OrderDto
    {
   
        public string timeOfWash { get; set; } = String.Empty;
        public string dateOfWash { get; set; } = String.Empty;
        public string location { get; set; } = String.Empty;
        public string code { get; set; } = String.Empty; //promocode
        //Payment details
        public string amountPaid { get; set; } = String.Empty;
        public string totalDiscount { get; set; } = String.Empty;


    }
}
