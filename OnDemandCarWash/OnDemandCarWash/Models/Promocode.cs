using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class Promocode
    {
        [Key]
        public int promoId { get; set; }
        public string code { get; set; }
        public string discount { get; set; }
        public string status { get; set; }
        public string timeStamp { get; set; } = String.Empty;

        //navigation properties
        //public virtual List<PaymentDetail> PaymentDetails { get; set; } 
    }
}
