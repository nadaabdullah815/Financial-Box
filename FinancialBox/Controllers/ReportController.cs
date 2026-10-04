using FinancialBox.Models;
using FinancialBox.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportController(IReportService reportService, UserManager<ApplicationUser> userManager)
        {
            _reportService = reportService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(DateTime? from, DateTime? to)
        {
            var userId = _userManager.GetUserId(User)!;

            // الفترة الافتراضية: من أول الشهر الحالي لليوم، إذا ما حدد المستخدم فترة
            var dateFrom = from ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var dateTo = to ?? DateTime.Now;

            var model = await _reportService.GetReportAsync(userId, dateFrom, dateTo);
            return View(model);
        }
    }
}