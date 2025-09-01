using System.Collections.Generic;

namespace RMI_IMS.Entities
{
    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReceivedDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Received, Cancelled

        public ICollection<PurchaseItem>? Items { get; set; }
    }
}