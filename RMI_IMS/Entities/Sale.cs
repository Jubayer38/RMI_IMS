namespace RMI_IMS.Entities
{

    public class Sale
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public decimal TotalAmount { get; set; }

        public ICollection<SaleItem>? Items { get; set; }
    }

}
