using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class Admin
    {

        [Key]
        public int userId { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
