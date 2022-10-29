namespace OnDemandCarWash.Dtos
{
    public class CustomerProfileDto
    {
        public int userId { get; set; }
        public string FirstName { get; set; } = string.Empty; // comes from users table
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;
    }
}
