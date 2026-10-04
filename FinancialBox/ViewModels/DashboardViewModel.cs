using FinancialBox.Models;

namespace FinancialBox.ViewModels
{
    public class DashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public List<Box> Boxes { get; set; } = new();
        public List<Transaction> RecentTransactions { get; set; } = new();
        public decimal MonthIncome { get; set; }
        public decimal MonthExpense { get; set; }
        public Box? DebtBox { get; set; }
    }
}