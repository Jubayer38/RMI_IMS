using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMI_IMS.Data;

namespace RMI_IMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/dashboard
        [HttpGet]
        public async Task<IActionResult> GetDashboardSummary()
        {
            // Reorder Alerts
            var reorderAlerts = await _context.InventoryItems
                .Where(i => !i.IsSold)
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    CurrentStock = g.Count(),
                    ReorderLevel = g.First().ReorderLevel,
                    NeedsReorder = g.Count() <= g.First().ReorderLevel
                })
                .Where(x => x.NeedsReorder)
                .ToListAsync();

            // Pending Warranty Claims
            var pendingClaims = await _context.WarrantyClaims
                .Where(c => c.Status == "Pending" || c.Status == "InProgress")
                .Include(c => c.Customer)
                .ToListAsync();

            // Recent Sales (last 7 days)
            var recentSales = await _context.Sales
                .Where(s => s.SaleDate >= DateTime.UtcNow.AddDays(-7))
                .Include(s => s.Customer)
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .ToListAsync();

            // Inventory Summary
            var inventorySummary = await _context.InventoryItems
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    TotalStock = g.Count(),
                    Sold = g.Count(i => i.IsSold),
                    Available = g.Count(i => !i.IsSold)
                })
                .ToListAsync();

            // Purchase Order Status
            var purchaseOrders = await _context.PurchaseOrders
                .Include(p => p.Supplier)
                .Include(p => p.Items)
                .Select(p => new
                {
                    p.PurchaseOrderId,
                    SupplierName = p.Supplier.Name,
                    p.OrderDate,
                    p.ReceivedDate,
                    p.Status,
                    ItemCount = p.Items.Count
                })
                .ToListAsync();

            return Ok(new
            {
                ReorderAlerts = reorderAlerts,
                PendingWarrantyClaims = pendingClaims,
                RecentSales = recentSales,
                InventorySummary = inventorySummary,
                PurchaseOrders = purchaseOrders
            });
        }
    }
}