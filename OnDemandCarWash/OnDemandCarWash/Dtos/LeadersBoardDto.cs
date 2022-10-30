namespace OnDemandCarWash.Dtos
{
    public class LeadersBoardDto
    {
        public int userId { get; set; } = 0;
        public string firstName { get; set; } = string.Empty; // further we will put only name
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public int waterUsed { get; set; } = 0; 
    }
}
