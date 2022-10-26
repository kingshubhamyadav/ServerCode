namespace OnDemandCarWash.Dtos
{
    public class WasherRequestsDto
    {
        public int orderId { get; set; }    //comes from Orders
        public string firstName { get; set; } = string.Empty;    //comes from Users
        public string lastName { get; set; } = string.Empty;    //comes from Users
        public string timeOfWash { get; set; } = string.Empty;  //comes from Orders
        public string dateOfWash { get; set; } = string.Empty;  //comes from Orders
        public string location { get; set; } = string.Empty;    //comes from Orders
        public string category { get; set; } = string.Empty;    //comes from WashType
    }
}
