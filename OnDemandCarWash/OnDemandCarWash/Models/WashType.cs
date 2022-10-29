using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class WashType
    {
        [Key]
        public int washTypeId { get; set; }
        public string categories { get; set; }
        public string discription { get; set; } = string.Empty;
        public string charges { get; set; }
        public string timeStamp { get; set; } = String.Empty;
        // public virtual List<Order> Orders { get; set; }
    }
}
