using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class CarDetail
    {
        public int orderId { get; set; }
        public string carNumber { get; set; }
        public string carType { get; set; }
        public string carImg { get; set; }
        public string timeStamp { get; set; }

        //navigation property
        public virtual Order Orders { get; set; }
    }
}
