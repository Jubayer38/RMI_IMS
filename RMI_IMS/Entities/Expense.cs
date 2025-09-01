namespace RMI_IMS.Entities
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } // e.g., Utilities, Rent, Salaries
    }
}