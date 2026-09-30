namespace FinancialBox.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; } 
        public DateTime Date { get; set; }
        public int BoxId { get; set; }
        public Box Box { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
      
    }
}