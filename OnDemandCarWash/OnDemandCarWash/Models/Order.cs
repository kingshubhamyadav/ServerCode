using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class Order
    {
        [Key]
        public int orderId { get; set; }
        public int userId { get; set; }//customer user id
        public string washerUsesrId { get; set; }
        public string timeOfWash { get; set; }
        public string dateOfWash { get; set; }
        public string location { get; set; }
        public string washTypeId { get; set; }
        public string rating { get; set; }
        public string orderStatus { get; set; }
        public string timeStamp { get; set; }

        //Navigation Property
        //public virtual User User { get; set; }
        //public virtual CarDetail CarDetail { get; set; }
        //public virtual AfterWash AfterWash { get; set; }
        //public virtual WashType WashTypes { get; set; }
        //public virtual List<PaymentDetail> PaymentDetails { get; set; }
    }
}
