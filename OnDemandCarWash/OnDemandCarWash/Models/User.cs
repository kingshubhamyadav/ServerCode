using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string role { get; set; }
        public string img { get; set; }
       // public string addressId { get; set; }
        public string status { get; set; }
        public string timeStamp { get; set; }

        //Navigation property
        public virtual Address Address { get; set; }
        public virtual List<Order> Orders { get; set; }

    }
}
