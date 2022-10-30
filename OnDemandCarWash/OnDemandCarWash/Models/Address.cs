

using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class Address
    {
        [Key]
        public int userId { get; set; }
      //  public int userId { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string pincode { get; set; }
        public string city { get; set; }  
        public string state { get; set; }
        public string timeStamp { get; set; } = DateTime.Now.ToString();

        //Navigation property
        //public virtual User User { get; set; }
    }
}
