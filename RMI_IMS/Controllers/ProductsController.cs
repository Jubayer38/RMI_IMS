using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMI_IMS.Data;
using RMI_IMS.Entities;

namespace RMI_IMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/products/{serialNumber}
        [HttpGet("{serialNumber}")]
        public async Task<ActionResult<Product>> GetProductBySerial(string serialNumber)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.SerialNumber == serialNumber);

            if (product == null)
                return NotFound($"Product with Serial Number '{serialNumber}' not found.");

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            // Check for duplicate serial number
            var exists = await _context.Products.AnyAsync(p => p.SerialNumber == product.SerialNumber);
            if (exists)
                return BadRequest("A product with this serial number already exists.");

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductBySerial), new { serialNumber = product.SerialNumber }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            // Update fields
            product.Name = updatedProduct.Name;
            product.Brand = updatedProduct.Brand;
            product.Model = updatedProduct.Model;
            product.Color = updatedProduct.Color;
            product.Storage = updatedProduct.Storage;
            product.SerialNumber = updatedProduct.SerialNumber;
            product.IsAccessory = updatedProduct.IsAccessory;
            product.Price = updatedProduct.Price;
            product.WarrantyPeriodMonths = updatedProduct.WarrantyPeriodMonths;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}