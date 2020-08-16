using AppGreat.Data;
using AppGreat.Models;
using AppGreat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppGreat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        public OrdersController(AppGreatDbContext context)
            : base(context) { }

        // GET: api/Orders/5
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Order> GetUserOrder(int id)
        {
            var currentUser = this.Context.Users.Where(u => u.Id == id).FirstOrDefault();
            var order = this.Context.Orders.Where(o => o.UserId == id).FirstOrDefault();

            if (order == null || currentUser == null)
            {
                return NotFound();
            }

            var exchangeRate = ExchangeRateService.GetExchangeRate(currentUser.CurrencyCode).Result;

            var productOrders = this.Context.ProductOrders.Where(a => a.OrderId == order.Id).ToList();

            foreach (var productOrder in productOrders)
            {
                var currentProduct = this.Context.Products.Where(p => p.Id == productOrder.ProductId).FirstOrDefault();
                order.Products.Add(currentProduct);
                order.TotalPrice += currentProduct.Price;

                currentProduct.Price = Math.Round(currentProduct.Price * exchangeRate, 2);
            }

            order.TotalPrice = Math.Round(exchangeRate * order.TotalPrice, 2);

            return order;
        }

        // PUT: api/Orders/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> OrderChangeStatus(int id, OrderStatusModel orderStatusModel)
        {
            var currentOrder = this.Context.Orders.Where(o => o.Id == id).FirstOrDefault();

            if (currentOrder == null)
            {
                return BadRequest();
            }

            currentOrder.Status = (Status)orderStatusModel.Status;

            await this.Context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Orders 
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderModel order)
        {
            var currentOder = this.Context.Orders.Where(o => o.UserId == order.UserId).FirstOrDefault();

            if (currentOder != null)
            {
                var currentProductOrder = new ProductOrder() { OrderId = currentOder.Id, ProductId = order.ProductId };

                this.Context.ProductOrders.Add(currentProductOrder);
            }
            else
            {
                currentOder = new Order() { UserId = order.UserId, CreatedAt = DateTime.Now, Status = Status.New };
                this.Context.Orders.Add(currentOder);
                await this.Context.SaveChangesAsync();

                var currentProcutOrder = new ProductOrder() { OrderId = currentOder.Id, ProductId = order.ProductId };
                this.Context.ProductOrders.Add(currentProcutOrder);
            }

            await this.Context.SaveChangesAsync();

            return currentOder;
        }
    }
}
