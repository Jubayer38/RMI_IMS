namespace RMI_IMS.Entities
{
    public class WarrantyClaim
    {
        public int WarrantyClaimId { get; set; }

        public string SerialNumber { get; set; } // Link to sold item
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime ClaimDate { get; set; } = DateTime.UtcNow;
        public string IssueDescription { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Resolved, Rejected
        public string ResolutionNotes { get; set; }
        public DateTime? ResolvedDate { get; set; }
    }
}