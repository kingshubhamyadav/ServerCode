namespace OnDemandCarWash.Dtos
{
    public class WasherProfileDto
    {
        public int userId { get; set; }
        public string FirstName { get; set; } = string.Empty; // comes from users table
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty; //comes from address table
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;
    }
}
