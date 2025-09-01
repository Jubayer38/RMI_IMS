namespace RMI_IMS.Entities
{
    public class PurchaseItem
    {
        public int PurchaseItemId { get; set; }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string? SerialNumber { get; set; } // Unique per item
        public decimal UnitCost { get; set; }
        public string? Location { get; set; } // Where it's stored
    }
}