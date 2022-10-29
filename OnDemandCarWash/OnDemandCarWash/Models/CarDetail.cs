using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class CarDetail
    {
        [Key]
        public int carDetailId { get; set; }
        public int orderId { get; set; }
        public string carNumber { get; set; }
        public string carType { get; set; }
        public string carImg { get; set; }
        public string timeStamp { get; set; } = String.Empty;

        //navigation property
        //public virtual Order Orders { get; set; }
    }
}
