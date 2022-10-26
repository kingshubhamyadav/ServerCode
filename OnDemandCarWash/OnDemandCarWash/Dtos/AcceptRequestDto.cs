namespace OnDemandCarWash.Dtos
{
    public class AcceptRequestDto
    {
        public int orderId { get; set; }
        public string washerId { get; set; } = string.Empty;
    }
}
