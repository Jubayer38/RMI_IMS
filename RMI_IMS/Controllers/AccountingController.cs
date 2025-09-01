using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMI_IMS.Data;
using RMI_IMS.Entities;

namespace RMI_IMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountingController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/accounting/invoice
        [HttpPost("invoice")]
        public async Task<ActionResult<Invoice>> CreateInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return Ok(invoice);
        }

        // GET: api/accounting/invoices
        [HttpGet("invoices")]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetAllInvoices()
        {
            return await _context.Invoices.ToListAsync();
        }

        // POST: api/accounting/expense
        [HttpPost("expense")]
        public async Task<ActionResult<Expense>> CreateExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return Ok(expense);
        }

        // GET: api/accounting/expenses
        [HttpGet("expenses")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpenses()
        {
            return await _context.Expenses.ToListAsync();
        }

        // GET: api/accounting/financial-report
        [HttpGet("financial-report")]
        public async Task<IActionResult> GetFinancialReport()
        {
            var totalSales = await _context.Invoices
                .Where(i => i.InvoiceType == "Sales")
                .SumAsync(i => i.TotalAmount);

            var totalPurchases = await _context.Invoices
                .Where(i => i.InvoiceType == "Purchase")
                .SumAsync(i => i.TotalAmount);

            var totalExpenses = await _context.Expenses
                .SumAsync(e => e.Amount);

            var netProfit = totalSales - (totalPurchases + totalExpenses);

            return Ok(new
            {
                TotalSales = totalSales,
                TotalPurchases = totalPurchases,
                TotalExpenses = totalExpenses,
                NetProfit = netProfit
            });
        }
    }
}