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
           var box = await _context.FinancialBoxes.FirstOrDefaultAsync(b => b.Id == boxId && b.UserId == userId);
       
           ViewBag.BoxId = boxId;
           ViewBag.Box = box;
           return View(transactions);
       }

        [HttpGet]
        public async Task<IActionResult> Create(int boxId= 0, string? type = null)
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
              await FillListsAsync(model, userId);
            return View(model);
        }

        private async Task FillListsAsync(TransactionViewModel model, string userId)
{
    model.UserBoxes = await _context.FinancialBoxes
        .Where(b => b.UserId == userId)
        .OrderBy(b => b.Id)
        .ToListAsync();

    model.Categories = await _context.Categories
        .Where(c => c.UserId == null || c.UserId == userId)
        .ToListAsync();
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