using FinancialBox.Data;
using FinancialBox.Models;
using FinancialBox.Models.Enum;
using FinancialBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var user = await _userManager.GetUserAsync(User);

            var boxes = await _context.FinancialBoxes
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.Id)
                .ToListAsync();

            var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var monthTransactions = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Box)
              .Where(t => t.Box.UserId == userId && t.Date >= monthStart && !t.Box.IsDebtBox)
                .ToListAsync();

            var recent = await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Box)
                .Where(t => t.Box.UserId == userId && t.Box.Name != "تسوية الديون")
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .Take(6)
                .ToListAsync();

            var model = new DashboardViewModel
            {
                FullName = user?.FullName ?? "مستخدم",
                Boxes = boxes,
                RecentTransactions = recent,
                MonthIncome = monthTransactions.Where(t => t.Category.Type == CategoryType.Income).Sum(t => t.Amount),
                MonthExpense = monthTransactions.Where(t => t.Category.Type == CategoryType.Expense).Sum(t => t.Amount),
                DebtBox = boxes.FirstOrDefault(b => b.Name == "تسوية الديون")
            };

            return View(model);
        }
    }
}