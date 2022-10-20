using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class PaymentDetail
    {
        [Key]
        public int paymentId { get; set; }
        public int transactionId { get; set; }
        public string paidAmount { get; set; }
        public string orderId { get; set; }
        public string status { get; set; }
        public string promoId { get; set; }
        public string totalDiscount { get; set; }
        public string pendingAmount { get; set; }
        public string timeStamp { get; set; }

        //navigation property
        //public virtual Order Orders { get; set; }

        //public virtual Promocode Promocodes { get; set; }


  }
}