namespace OnDemandCarWash.Dtos
{
    public class WasherRequestsDto
    {
        public int orderId { get; set; }
        public int userId { get; set; }
        public string timeOfWash { get; set; } = string.Empty;
        public string dateOfWash { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public string washTypeId { get; set; } = string.Empty;
    }
}
