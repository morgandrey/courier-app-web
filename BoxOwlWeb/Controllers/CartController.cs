using System;
using System.Linq;
using System.Threading.Tasks;
using BoxOwlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using BoxOwlWeb.Models.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOwlWeb.Controllers {
    public class CartController : Controller {

        private readonly BoxOwlDbContext dbContext;
        private readonly ILogger<AccountController> logger;

        public CartController(BoxOwlDbContext dbContext, ILogger<AccountController> logger) {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public IActionResult Index() {
            return View(ProductList.Products);
        }

        public IActionResult DeleteCart() {
            ProductList.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ConfirmOrder() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(string deliveryAddress, string orderDescription) {
            try {
                var transaction = await dbContext.Database.BeginTransactionAsync();
                var currentUser = await dbContext.Client.FirstOrDefaultAsync(x => x.ClientEmail == User.Identity.Name);
                var totalSum = ProductList.Products.Sum(t => t.ProductCost);
                var order = new Order {
                    ClientId = currentUser.ClientId,
                    OrderDate = DateTime.Now,
                    OrderDescription = orderDescription,
                    DeliveryAddress = deliveryAddress,
                    OrderStatusId = 1,
                    OrderCost = totalSum,
                    CourierReward = 0.05f * totalSum
                };
                if (totalSum <= 1500) {
                    order.OrderRating = 5;
                } else if (totalSum > 1500 && totalSum <= 4000) {
                    order.OrderRating = 10;
                } else {
                    order.OrderRating = 15;
                }
                await dbContext.AddAsync(order);
                await dbContext.SaveChangesAsync();

                for (int i = 0; i < ProductList.Products.Count; i++) {
                    var productOrder = new ProductOrder {
                        ProductId = ProductList.Products[i].ProductId,
                        OrderId = order.OrderId
                    };
                    await dbContext.AddAsync(productOrder);
                }
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            } catch (Exception ex) {
                logger.LogTrace(ex.Message);
            }
            return View();
        }
    }
}
