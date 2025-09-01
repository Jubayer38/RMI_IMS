namespace RMI_IMS.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int LoyaltyPoints { get; set; } = 0;
    }
}