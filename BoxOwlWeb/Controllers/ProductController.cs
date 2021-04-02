using System.Linq;
using System.Threading.Tasks;
using BoxOwlWeb.Models;
using BoxOwlWeb.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOwlWeb.Controllers {
    public class ProductController : Controller {

        private readonly BoxOwlDbContext dbContext;
        private readonly ILogger<AccountController> logger;

        public ProductController(BoxOwlDbContext dbContext, ILogger<AccountController> logger) {
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public async Task<IActionResult> Index() {
            ViewBag.ProductList = ProductList.Products;
            return View(await dbContext.Product
                .Take(50)
                .ToListAsync());
        }

        public async Task<IActionResult> AddProductToCart(int productId) {
            var product = await dbContext.Product.FindAsync(productId);
            ProductList.Add(product);
            return RedirectToAction("Index");
        }
    }
}
