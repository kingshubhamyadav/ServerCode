using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class WashType
    {
        [Key]
        public int washTypeId { get; set; }
        public string catogries { get; set; } = string.Empty;
        public string Discription { get; set; } = string.Empty;
        public string charges { get; set; } = string.Empty;
        public string timeStamp { get; set; } = string.Empty;   
       // public virtual List<Order> Orders { get; set; }
    }
}
