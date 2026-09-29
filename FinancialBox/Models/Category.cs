using System;
using FinancialBox.Models.Enum;
namespace FinancialBox.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CategoryType Type { get; set; } // Income / Expense
         public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}