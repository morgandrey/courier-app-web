using System;
using System.Collections.Generic;

namespace BoxOwlWeb.Models
{
    public partial class Order
    {
        public Order()
        {
            ProductOrder = new HashSet<ProductOrder>();
        }

        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public int? CourierId { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderDescription { get; set; }
        public int OrderStatusId { get; set; }
        public int OrderRating { get; set; }
        public float OrderCost { get; set; }
        public float CourierReward { get; set; }

        public virtual Client Client { get; set; }
        public virtual Courier Courier { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
