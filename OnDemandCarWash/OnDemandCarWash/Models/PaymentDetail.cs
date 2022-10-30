using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class PaymentDetail
    {
        [Key]
        public int paymentId { get; set; }
        public int orderId { get; set; } = 0;
        public string amountPaid { get; set; } = String.Empty;
        public string paymentStatus { get; set; } = String.Empty;
        public string totalDiscount { get; set; } = String.Empty;
        public string timeStamp { get; set; } = DateTime.Now.ToString();

        //navigation property
        //public virtual Order Orders { get; set; }

        //public virtual Promocode Promocodes { get; set; }


    }
}