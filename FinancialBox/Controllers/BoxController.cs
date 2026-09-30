using FinancialBox.Services.Interfaces;
using FinancialBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinancialBox.Models;

namespace FinancialBox.Controllers
{
    [Authorize]
    public class BoxController : Controller
    {
        private readonly IBoxService _boxService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BoxController(IBoxService boxService, UserManager<ApplicationUser> userManager)
        {
            _boxService = boxService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var boxes = await _boxService.GetUserBoxesAsync(userId);
            return View(boxes);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View(new BoxViewModel { Currency = "SYP" });
        }

        [HttpPost]
        public async Task<IActionResult> Create(BoxViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = _userManager.GetUserId(User)!;
            await _boxService.CreateAsync(userId, model.Name, model.Currency, model.Balance);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var box = await _boxService.GetByIdAsync(id, userId);
            if (box == null) return NotFound();

            var model = new BoxViewModel { Id = box.Id, Name = box.Name, Currency = box.Currency, Balance = box.Balance };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BoxViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = _userManager.GetUserId(User)!;
            var (success, message) = await _boxService.UpdateAsync(model.Id, userId, model.Name, model.Currency);

            if (!success)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            
            TempData["SuccessMessage"] = message;
            return RedirectToAction(nameof(Index));
        }

      [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var box = await _boxService.GetByIdAsync(id, userId);
            if (box == null) return NotFound();
        
            if (box.IsDefault)
            {
                TempData["ErrorMessage"] = "لا يمكن حذف هذا الصندوق لأنه صندوق أساسي بالنظام";
                return RedirectToAction(nameof(Index));
            }
        
            return View(box);
        }
        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var (success, message) = await _boxService.DeleteAsync(id, userId);
        
            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}