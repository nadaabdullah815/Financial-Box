using FinancialBox.Data;
using FinancialBox.Models.Enum;
using FinancialBox.Services.Interfaces;
using FinancialBox.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReportViewModel> GetReportAsync(string userId, DateTime from, DateTime to)
        {
            // نضيف يوم كامل لـ "إلى" عشان نشمل كل عمليات آخر يوم بالفترة
            var toInclusive = to.Date.AddDays(1).AddTicks(-1);

            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Box)
                .Where(t => t.Box.UserId == userId && t.Date >= from.Date && t.Date <= toInclusive && !t.Box.IsDebtBox)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var income = transactions.Where(t => t.Category.Type == CategoryType.Income).ToList();
            var expense = transactions.Where(t => t.Category.Type == CategoryType.Expense).ToList();

            var boxes = await _context.FinancialBoxes
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.Id)
                .ToListAsync();

            var incomeByCategory = income
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategorySummaryItem { CategoryName = g.Key, Total = g.Sum(x => x.Amount), Count = g.Count() })
                .OrderByDescending(x => x.Total)
                .ToList();

            var expenseByCategory = expense
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategorySummaryItem { CategoryName = g.Key, Total = g.Sum(x => x.Amount), Count = g.Count() })
                .OrderByDescending(x => x.Total)
                .ToList();

            return new ReportViewModel
            {
                DateFrom = from.Date,
                DateTo = to.Date,
                IncomeTransactions = income,
                ExpenseTransactions = expense,
                Boxes = boxes,
                IncomeByCategory = incomeByCategory,
                ExpenseByCategory = expenseByCategory
            };
        }
    }
}