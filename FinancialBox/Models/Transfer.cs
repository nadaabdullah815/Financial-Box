namespace FinancialBox.Models
{
    public class Transfer
    {
        public int Id { get; set; }

        public int FromBoxId { get; set; }
        public Box FromBox { get; set; } = null!;

        public int ToBoxId { get; set; }
        public Box ToBox { get; set; } = null!;

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }

        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}