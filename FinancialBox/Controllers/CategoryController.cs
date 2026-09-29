using FinancialBox.Models;
using FinancialBox.Models.Enum;
using FinancialBox.Services.Interfaces;
using FinancialBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ICategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            _categoryService = categoryService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var categories = await _categoryService.GetUserCategoriesAsync(userId);
            return View(categories);
        }

      [HttpGet]
public IActionResult Create(string? type)
{
    var model = new CategoryViewModel {  Type = "Expense" };
    return View(model);
}

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = _userManager.GetUserId(User)!;
            var type = model.Type == "Income" ? CategoryType.Income : CategoryType.Expense;
            var (success, message) = await _categoryService.CreateAsync(userId, model.Name, type);

            if (!success)
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var category = await _categoryService.GetByIdAsync(id, userId);
            if (category == null) return NotFound(); // إما مش موجود أو عام (محمي)

            var model = new CategoryViewModel { Id = category.Id, Name = category.Name, Type = category.Type.ToString() };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = _userManager.GetUserId(User)!;
            var type = model.Type == "Income" ? CategoryType.Income : CategoryType.Expense;
             var (success, message) = await _categoryService.UpdateAsync(model.Id, userId, model.Name, type);

            if (!success)
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            await _categoryService.DeleteAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}