using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; }=string.Empty;
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } 
        public byte[] PasswordSalt { get; set; }
        public string phone { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;    
        public string img { get; set; } = string.Empty;
        // public string addressId { get; set; }
        public string status { get; set; }=string.Empty;
        public string timeStamp { get; set; } = DateTime.Now.ToString();

        //Navigation property
        //public virtual Address Address { get; set; }
        //public virtual List<Order> Orders { get; set; }

    }
}
