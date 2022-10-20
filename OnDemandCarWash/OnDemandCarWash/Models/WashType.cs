using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class WashType
    {
        [Key]
        public int washTypeId { get; set; }
        public int catogries { get; set; }
        public string charges { get; set; }
        public string timeStamp { get; set; }
       // public virtual List<Order> Orders { get; set; }
    }
}
