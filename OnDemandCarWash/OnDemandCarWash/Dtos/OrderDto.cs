namespace OnDemandCarWash.Dtos
{
    public class OrderDto
    {

        //car details
        public string carNumber { get; set; } = String.Empty;
        public string carType { get; set; } = String.Empty;
        public string carImg { get; set; } = String.Empty;
        public int userId { get; set; } = 0;
        public int washerUserId { get; set; } = 0;


        public string timeOfWash { get; set; } = String.Empty;
        public string dateOfWash { get; set; } = String.Empty;
        public string location { get; set; } = String.Empty;
        public string code { get; set; } = String.Empty; //promocode
        //Payment details
        public string amountPaid { get; set; } = String.Empty;
        public string totalDiscount { get; set; } = String.Empty;


    }
}
