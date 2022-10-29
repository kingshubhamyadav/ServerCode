namespace OnDemandCarWash.Dtos
{
    public class GetPromocodeDto
    {
        public int promoId { get; set; } 
        public string code { get; set; } = String.Empty;
        public string discount { get; set; } = String.Empty;
        public string status { get; set; } = String.Empty;
    }
}
