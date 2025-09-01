namespace RMI_IMS.Entities
{

    public class SaleItem
    {
        public int SaleItemId { get; set; }

        public int SaleId { get; set; }
        public Sale? Sale { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public string? SerialNumber { get; set; } // Sold item
        public decimal UnitPrice { get; set; }
    }

}
