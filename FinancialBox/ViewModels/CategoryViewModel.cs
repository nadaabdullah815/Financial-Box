using System.ComponentModel.DataAnnotations;

namespace FinancialBox.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty; // "Income" أو "Expense"
    }
}