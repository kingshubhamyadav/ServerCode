namespace OnDemandCarWash.Dtos
{
    public class AdminWashRequestDto
    {
        public int orderId { get; set; }
        public string code { get; set; }
        public string timeOfWash { get; set; }
        public string dateOfWash { get; set; }
        public string location { get; set; }
        public string package { get; set; }
        public string userMail { get; set; }

    }
}
