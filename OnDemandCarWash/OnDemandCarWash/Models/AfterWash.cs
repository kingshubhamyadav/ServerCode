using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class AfterWash
    {

        [Key]
        public int washId { get; set; }
        public int orderId { get; set; }
        public int waterUsed { get; set; }
        public string carImg { get; set; }
        public string timeStamp { get; set; }=String.Empty;

        //navigation property
        //public virtual Order Orders { get; set; }

    }
}
