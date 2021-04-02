using System;
using System.Collections.Generic;

namespace BoxOwlWeb.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductOrder = new HashSet<ProductOrder>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float ProductCost { get; set; }

        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
