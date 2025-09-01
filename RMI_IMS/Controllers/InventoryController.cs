using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMI_IMS.Data;
using RMI_IMS.Entities;

namespace RMI_IMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetAllInventory()
        {
            return await _context.InventoryItems.Include(i => i.Product).ToListAsync();
        }

        // GET: api/inventory/{serialNumber}
        [HttpGet("{serialNumber}")]
        public async Task<ActionResult<Inventory>> GetBySerial(string serialNumber)
        {
            var item = await _context.InventoryItems
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.SerialNumber == serialNumber);

            if (item == null)
                return NotFound($"Item with Serial Number '{serialNumber}' not found.");

            return Ok(item);
        }

        // POST: api/inventory
        [HttpPost]
        public async Task<ActionResult<Inventory>> AddInventoryItem(Inventory inventory)
        {
            var exists = await _context.InventoryItems.AnyAsync(i => i.SerialNumber == inventory.SerialNumber);
            if (exists)
                return BadRequest("An item with this serial number already exists in inventory.");

            _context.InventoryItems.Add(inventory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBySerial), new { serialNumber = inventory.SerialNumber }, inventory);
        }

        // PUT: api/inventory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryItem(int id, Inventory updatedItem)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null)
                return NotFound();

            item.Location = updatedItem.Location;
            item.IsSold = updatedItem.IsSold;
            item.ReorderLevel = updatedItem.ReorderLevel;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/inventory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}