using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMI_IMS.Data;
using RMI_IMS.Entities;

namespace RMI_IMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/sales
        [HttpPost]
        public async Task<ActionResult<Sale>> CreateSale(Sale sale)
        {
            // Validate serial numbers and update inventory
            foreach (var item in sale.Items)
            {
                var inventoryItem = await _context.InventoryItems
                    .FirstOrDefaultAsync(i => i.SerialNumber == item.SerialNumber && !i.IsSold);

                if (inventoryItem == null)
                    return BadRequest($"Item with Serial Number '{item.SerialNumber}' is not available or already sold.");

                inventoryItem.IsSold = true;
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return Ok(sale);
        }

        // GET: api/sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetAllSales()
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Employee)
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        // GET: api/sales/warranty/{serialNumber}
        [HttpGet("warranty/{serialNumber}")]
        public async Task<IActionResult> CheckWarranty(string serialNumber)
        {
            var saleItem = await _context.SaleItems
                .Include(si => si.Sale)
                .Include(si => si.Product)
                .FirstOrDefaultAsync(si => si.SerialNumber == serialNumber);

            if (saleItem == null)
                return NotFound($"No sale record found for Serial Number '{serialNumber}'.");

            var saleDate = saleItem.Sale.SaleDate;
            var warrantyMonths = saleItem.Product.WarrantyPeriodMonths;
            var expiryDate = saleDate.AddMonths(warrantyMonths);
            var isUnderWarranty = DateTime.UtcNow <= expiryDate;

            return Ok(new
            {
                SerialNumber = serialNumber,
                ProductName = saleItem.Product.Name,
                SaleDate = saleDate,
                WarrantyMonths = warrantyMonths,
                WarrantyExpiry = expiryDate,
                IsUnderWarranty = isUnderWarranty
            });
        }
    }
}