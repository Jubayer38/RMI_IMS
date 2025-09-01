using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMI_IMS.Data;
using RMI_IMS.Entities;

namespace RMI_IMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PurchaseController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/purchase/supplier
        [HttpPost("supplier")]
        public async Task<ActionResult<Supplier>> AddSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return Ok(supplier);
        }

        // POST: api/purchase/order
        [HttpPost("order")]
        public async Task<ActionResult<PurchaseOrder>> CreatePurchaseOrder(PurchaseOrder order)
        {
            _context.PurchaseOrders.Add(order);
            await _context.SaveChangesAsync();
            return Ok(order);
        }

        // POST: api/purchase/item
        [HttpPost("item")]
        public async Task<ActionResult<PurchaseItem>> AddPurchaseItem(PurchaseItem item)
        {
            // Check if serial number already exists in inventory
            var exists = await _context.InventoryItems.AnyAsync(i => i.SerialNumber == item.SerialNumber);
            if (exists)
                return BadRequest("Item with this serial number already exists in inventory.");

            // Add purchase item
            _context.PurchaseItems.Add(item);

            // Add to inventory
            var inventoryItem = new Inventory
            {
                ProductId = item.ProductId,
                SerialNumber = item.SerialNumber,
                Location = item.Location,
                IsSold = false,
                ReorderLevel = 5 // default, can be customized
            };

            _context.InventoryItems.Add(inventoryItem);

            await _context.SaveChangesAsync();
            return Ok(item);
        }

        // GET: api/purchase/orders
        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetAllOrders()
        {
            return await _context.PurchaseOrders
                .Include(o => o.Supplier)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        // GET: api/purchase/suppliers
        [HttpGet("suppliers")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliers()
        {
            return await _context.Suppliers.ToListAsync();
        }
    }
}