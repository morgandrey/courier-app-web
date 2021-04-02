using System.Collections.Generic;

namespace BoxOwlWeb.Models.CustomModels {
    public class ProductList {
        public static List<Product> Products;
        static ProductList() {
            Products = new List<Product>();
        }
        public static void Add(Product book) {
            Products.Add(book);
        }
        public static void Clear() {
            Products.Clear();
        }
    }
}