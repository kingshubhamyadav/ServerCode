namespace OnDemandCarWash.Dtos
{
    public class AcceptRequestDto
    {
        public int orderId { get; set; }
        public int washerId { get; set; } = 0;
    }
}
