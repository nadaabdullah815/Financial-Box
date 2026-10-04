using FinancialBox.Models;

namespace FinancialBox.ViewModels
{
    public class CategorySummaryItem
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int Count { get; set; }
    }

    public class ReportViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public List<Transaction> IncomeTransactions { get; set; } = new();
        public List<Transaction> ExpenseTransactions { get; set; } = new();
        public List<Box> Boxes { get; set; } = new();
        public List<CategorySummaryItem> IncomeByCategory { get; set; } = new();
        public List<CategorySummaryItem> ExpenseByCategory { get; set; } = new();

        public decimal TotalIncome => IncomeTransactions.Sum(t => t.Amount);
        public decimal TotalExpense => ExpenseTransactions.Sum(t => t.Amount);
    }
}