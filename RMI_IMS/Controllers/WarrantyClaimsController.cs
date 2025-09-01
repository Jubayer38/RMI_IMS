using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMI_IMS.Data;
using RMI_IMS.Entities;

namespace RMI_IMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarrantyClaimsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WarrantyClaimsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/warrantyclaims
        [HttpPost]
        public async Task<ActionResult<WarrantyClaim>> SubmitClaim(WarrantyClaim claim)
        {
            // Check if serial number is valid and sold
            var saleItem = await _context.SaleItems
                .Include(si => si.Sale)
                .Include(si => si.Product)
                .FirstOrDefaultAsync(si => si.SerialNumber == claim.SerialNumber);

            if (saleItem == null)
                return BadRequest("Invalid serial number or item not sold.");

            // Check warranty validity
            var expiryDate = saleItem.Sale.SaleDate.AddMonths(saleItem.Product.WarrantyPeriodMonths);
            if (DateTime.UtcNow > expiryDate)
                return BadRequest("Warranty period has expired.");

            _context.WarrantyClaims.Add(claim);
            await _context.SaveChangesAsync();

            return Ok(claim);
        }

        // GET: api/warrantyclaims
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarrantyClaim>>> GetAllClaims()
        {
            return await _context.WarrantyClaims
                .Include(c => c.Customer)
                .ToListAsync();
        }

        // PUT: api/warrantyclaims/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClaimStatus(int id, WarrantyClaim updatedClaim)
        {
            var claim = await _context.WarrantyClaims.FindAsync(id);
            if (claim == null)
                return NotFound();

            claim.Status = updatedClaim.Status;
            claim.ResolutionNotes = updatedClaim.ResolutionNotes;
            claim.ResolvedDate = updatedClaim.ResolvedDate ?? DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}