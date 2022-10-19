using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class TotalRating
    {
        [Key]
        public int Id { get; set; }
        public int washerUserId { get; set; } //fK--Id of users Table
        public string totalRating { get; set; }
        public string waterSaved { get; set; }
    }
}
