using System.ComponentModel.DataAnnotations;

namespace FinancialBox.ViewModels
{
    public class BoxViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الصندوق مطلوب")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "العملة مطلوبة")]
        [RegularExpression("^(SYP|USD)$", ErrorMessage = "العملة غير مدعومة")]
        public string Currency { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "الرصيد لازم يكون صفر أو أكبر")]
        public decimal Balance { get; set; }
    }
}