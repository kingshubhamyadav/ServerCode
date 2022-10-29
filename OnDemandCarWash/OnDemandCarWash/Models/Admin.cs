using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class Admin
    {

        [Key]
        public string userId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
}
