using AppGreat.Data;
using AppGreat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGreat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        public ProductsController(AppGreatDbContext context)
            : base(context) { }


        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await this.Context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await this.Context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductModel product)
        {
            var currentProduct = new Product() { Name = product.Name, Price = decimal.Parse(product.Price) };

            this.Context.Products.Add(currentProduct);
            await this.Context.SaveChangesAsync();

            return currentProduct;
        }

        // DELETE: api/Products/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await Context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            this.Context.Products.Remove(product);
            await this.Context.SaveChangesAsync();

            return product;
        }
    }
}
