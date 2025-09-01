namespace RMI_IMS.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public string InvoiceType { get; set; } // Sales or Purchase
        public int RelatedId { get; set; } // SaleId or PurchaseOrderId
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmount { get; set; }
    }
}