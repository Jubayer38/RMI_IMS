namespace RMI_IMS.Entities
{
    public class Inventory
    {
        public int InventoryId { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string? SerialNumber { get; set; } // Unique per item
        public string? Location { get; set; }     // Optional: store/warehouse
        public bool IsSold { get; set; }         // True if sold
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public int ReorderLevel { get; set; }    // Minimum quantity before alert
    }
}