using FinancialBox.Models;
using FinancialBox.Models.Enum;
using FinancialBox.Services.Interfaces;
using FinancialBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FinancialBox.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public TransactionController(
            ITransactionService transactionService,
            UserManager<ApplicationUser> userManager,
            AppDbContext context)
        {
            _transactionService = transactionService;
            _userManager = userManager;
            _context = context;
        }

        // عرض كل عمليات صندوق معين
        public async Task<IActionResult> Index(int boxId)
        {
            var userId = _userManager.GetUserId(User)!;
            var transactions = await _transactionService.GetBoxTransactionsAsync(boxId, userId);

            ViewBag.BoxId = boxId;
            return View(transactions);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int boxId)
        {
            var userId = _userManager.GetUserId(User)!;

            var model = new TransactionViewModel
            {
                BoxId = boxId,
                Date = DateTime.Now,
                Categories = await _context.Categories
                    .Where(c => c.UserId == null || c.UserId == userId)
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionViewModel model)
        {
            var userId = _userManager.GetUserId(User)!;

            if (!ModelState.IsValid)
            {
                model.Categories = await _context.Categories
                    .Where(c => c.UserId == null || c.UserId == userId)
                    .ToListAsync();
                return View(model);
            }

            var type = model.Type == "Income" ? CategoryType.Income : CategoryType.Expense;

            var (success, message) = await _transactionService.AddTransactionAsync(
                userId, model.BoxId, model.CategoryId, model.Amount, type, model.Description, model.Date);

            if (!success)
            {
                ModelState.AddModelError("", message);
                model.Categories = await _context.Categories
                    .Where(c => c.UserId == null || c.UserId == userId)
                    .ToListAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index), new { boxId = model.BoxId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int boxId)
        {
            var userId = _userManager.GetUserId(User)!;
            await _transactionService.DeleteTransactionAsync(id, userId);
            return RedirectToAction(nameof(Index), new { boxId });
        }
    }
}