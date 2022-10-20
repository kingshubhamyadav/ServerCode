using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class Order
    {
        [Key]
        public int orderId { get; set; }
        public int userId { get; set; } =0; //customer user id
        public int washerUserId { get; set; } = 0;
        public string code { get; set; }=String.Empty;//promocode
        public string timeOfWash { get; set; } = String.Empty;
        public string dateOfWash { get; set; } = String.Empty;
        public string location { get; set; } = String.Empty;
        public int washTypeId { get; set; } = 0;
        public string rating { get; set; } = String.Empty;
        public string orderStatus { get; set; } = String.Empty; //enum
        public string timeStamp { get; set; } = String.Empty;

        //Navigation Property
        //public virtual User User { get; set; }
        //public virtual CarDetail CarDetail { get; set; }
        //public virtual AfterWash AfterWash { get; set; }
        //public virtual WashType WashTypes { get; set; }
        //public virtual List<PaymentDetail> PaymentDetails { get; set; }
    }
}
