﻿using System.ComponentModel.DataAnnotations;

namespace OnDemandCarWash.Models
{
    public class AfterWash
    {
     
        public int orderId { get; set; }
        public int waterUsed { get; set; }
        public string carImg { get; set; }
        public string timeStamp { get; set; }

        //navigation property
        public virtual Order Orders { get; set; }

    }
}