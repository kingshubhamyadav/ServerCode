using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class Promocode
    {
        [Key]
        public int promoId { get; set; }
        public string code { get; set; }=String.Empty;
        public string discount { get; set; } = String.Empty;
        public string status { get; set; } = String.Empty;
        public string timeStamp { get; set; } = String.Empty;

        //navigation properties
        //public virtual List<PaymentDetail> PaymentDetails { get; set; } 
    }
}
