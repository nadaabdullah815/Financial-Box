using System.ComponentModel.DataAnnotations;

namespace FinancialBox.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }

        [Required]
        public int BoxId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Range(0.01, double.MaxValue, ErrorMessage = "المبلغ يجب أن يكون أكبر من صفر")]
        public decimal Amount { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty; // "Income" أو "Expense"

        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        // للعرض بالـ Views (Dropdowns)
        public List<FinancialBox.Models.Box>? UserBoxes { get; set; }
        public List<FinancialBox.Models.Category>? Categories { get; set; }
    }
}