using System;
using System.Collections.Generic;

namespace BoxOwlWeb.Models
{
    public partial class Courier
    {
        public Courier()
        {
            Order = new HashSet<Order>();
        }

        public int CourierId { get; set; }
        public string CourierName { get; set; }
        public string CourierSurname { get; set; }
        public string CourierPhone { get; set; }
        public string CourierImage { get; set; }
        public string CourierPassword { get; set; }
        public string CourierSalt { get; set; }
        public int CourierRating { get; set; }
        public float CourierMoney { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
