using System.ComponentModel.DataAnnotations;
using FinancialBox.Models;

namespace FinancialBox.ViewModels
{
    public class TransferViewModel
    {
        [Required(ErrorMessage = "الصندوق المحوَّل منه مطلوب")]
        public int FromBoxId { get; set; }

        [Required(ErrorMessage = "الصندوق المحوَّل إليه مطلوب")]
        public int ToBoxId { get; set; }

        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Range(0.01, double.MaxValue, ErrorMessage = "المبلغ يجب أن يكون أكبر من صفر")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "التاريخ مطلوب")]
        public DateTime Date { get; set; } = DateTime.Now;

        public string? Description { get; set; }

        // للعرض فقط (القوائم المنسدلة)
        public List<Box>? Boxes { get; set; }
    }
}