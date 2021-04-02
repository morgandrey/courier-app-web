using System;
using System.Collections.Generic;

namespace BoxOwlWeb.Models
{
    public partial class ProductOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
