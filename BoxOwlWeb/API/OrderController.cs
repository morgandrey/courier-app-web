using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoxOwlWeb.Models;
using BoxOwlWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BoxOwlWeb.API {

    [ApiController]
    public class OrderController : ControllerBase {
        private readonly BoxOwlDbContext _context;
        public OrderController(BoxOwlDbContext context) {
            _context = context;

        }

        // GET: api/orders/{orderId}
        [HttpGet]
        [Route("api/orders/{orderId}")]
        public async Task<ActionResult<Order>> GetOrder(int orderId) {
            var order = await _context.Order
                .Include(x => x.Client)
                .Include(x => x.OrderStatus)
                .Include(x => x.ProductOrder)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);
            return Ok(ToOrderDto(order));
        }

        // GET: api/available-orders/{courierId}
        [HttpGet]
        [Route("api/available-orders/{courierId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAvailableOrders(int courierId) {
            var orders = await _context.Order
                .Include(x => x.Client)
                .Include(x => x.OrderStatus)
                .Include(x => x.ProductOrder)
                .ThenInclude(x => x.Product)
                .Where(x => x.OrderStatusId == 1)
                .OrderByDescending(x => x.OrderCost)
                .ToListAsync();
            var courier = await _context.Courier.FirstOrDefaultAsync(x => x.CourierId == courierId);
            if (courier.CourierRating <= 100) {
                return Ok(JsonConvert.SerializeObject(ToOrderListDTO(orders.Where(x => x.OrderCost <= 2500))));
            }
            if (courier.CourierRating > 100 && courier.CourierRating <= 250) {
                return Ok(JsonConvert.SerializeObject(ToOrderListDTO(orders.Where(x => x.OrderCost <= 5000))));
            }
            return Ok(ToOrderListDTO(orders));
        }

        private static List<OrderDto> ToOrderListDTO(IEnumerable<Order> orders) {
            return orders.Select(order => new OrderDto {
                OrderId = order.OrderId,
                ClientName = order.Client.ClientName,
                ClientSurname = order.Client.ClientSurname,
                ClientPhone = order.Client.ClientPhone,
                OrderStatusId = order.OrderStatus.IdOrderStatus,
                DeliveryAddress = order.DeliveryAddress,
                OrderDate = order.OrderDate,
                CourierId = order.CourierId,
                OrderDescription = order.OrderDescription,
                OrderRating = order.OrderRating,
                CourierReward = order.CourierReward,
                Products = order.ProductOrder.Select(product => new ProductDto {
                    ProductName = product.Product.ProductName,
                    ProductDescription = product.Product.ProductDescription,
                    ProductCost = product.Product.ProductCost
                })
            })
                .ToList();
        }

        private static OrderDto ToOrderDto(Order order) {
            return new OrderDto {
                OrderId = order.OrderId,
                ClientName = order.Client.ClientName,
                ClientSurname = order.Client.ClientSurname,
                ClientPhone = order.Client.ClientPhone,
                OrderStatusId = order.OrderStatus.IdOrderStatus,
                DeliveryAddress = order.DeliveryAddress,
                OrderDate = order.OrderDate,
                CourierId = order.CourierId,
                OrderDescription = order.OrderDescription,
                OrderRating = order.OrderRating,
                CourierReward = order.CourierReward,
                Products = order.ProductOrder.Select(product => new ProductDto {
                    ProductName = product.Product.ProductName,
                    ProductDescription = product.Product.ProductDescription,
                    ProductCost = product.Product.ProductCost
                })
            };
        }

        // POST: api/order/take
        [HttpPost]
        [Route("api/order/take")]
        public async Task<ActionResult> TakeOrder(OrderDto orderDto) {
            try {
                var order = await _context.Order.FirstOrDefaultAsync(x => x.OrderId == orderDto.OrderId);
                order.OrderStatusId = 2;
                order.CourierId = orderDto.CourierId;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            } catch (DbUpdateConcurrencyException) {
                return BadRequest();
            }
        }

        // POST: api/order/complete
        [HttpPost]
        [Route("api/order/complete")]
        public async Task<ActionResult<bool>> CompleteOrder(OrderDto orderDto) {
            try {
                var order = await _context.Order.FirstOrDefaultAsync(x => x.OrderId == orderDto.OrderId);
                var courier = await _context.Courier.FirstOrDefaultAsync(x => x.CourierId == order.CourierId);
                order.OrderStatusId = 3;
                courier.CourierRating += order.OrderRating;
                courier.CourierMoney += order.CourierReward;
                _context.Entry(order).State = EntityState.Modified;
                _context.Entry(courier).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(true);
            } catch (DbUpdateConcurrencyException) {
                return BadRequest();
            }
        }

        // GET: api/courier/{courierId}/history-orders
        [HttpGet]
        [Route("api/courier/{courierId}/history-orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetHistoryOrders(int courierId) {
            try {
                return Ok(JsonConvert.SerializeObject(ToOrderListDTO(await _context.Order
                    .Include(x => x.Client)
                    .Include(x => x.OrderStatus)
                    .Include(x => x.ProductOrder)
                    .ThenInclude(x => x.Product)
                    .Where(x => x.OrderStatusId == 3 && x.CourierId == courierId)
                    .ToListAsync())));
            } catch (DbUpdateConcurrencyException) {
                return BadRequest();
            }
        }

        // GET: api/courier/{courierId}/active-orders
        [HttpGet]
        [Route("api/courier/{courierId}/active-orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetActiveOrders(int courierId) {
            try {
                return Ok(JsonConvert.SerializeObject(ToOrderListDTO(await _context.Order
                    .Include(x => x.Client)
                    .Include(x => x.OrderStatus)
                    .Include(x => x.ProductOrder)
                    .ThenInclude(x => x.Product)
                    .Where(x => x.OrderStatusId == 2 && x.CourierId == courierId)
                    .ToListAsync())));
            } catch (DbUpdateConcurrencyException) {
                return BadRequest();
            }
        }
    }
}
