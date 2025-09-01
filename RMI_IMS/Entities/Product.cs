namespace RMI_IMS.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public string? Storage { get; set; }
        public string? SerialNumber { get; set; } 
        public bool IsAccessory { get; set; }
        public decimal Price { get; set; }
        public int WarrantyPeriodMonths { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}