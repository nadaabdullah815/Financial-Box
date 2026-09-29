using FinancialBox.Models;
using FinancialBox.Services.Interfaces;
using FinancialBox.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Controllers
{
    [Authorize]
    public class TransferController : Controller
    {
        private readonly ITransferService _transferService;
        private readonly IBoxService _boxService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransferController(
            ITransferService transferService,
            IBoxService boxService,
            UserManager<ApplicationUser> userManager)
        {
            _transferService = transferService;
            _boxService = boxService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var transfers = await _transferService.GetUserTransfersAsync(userId);
            return View(transfers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User)!;
            var model = new TransferViewModel
            {
                Boxes = await _boxService.GetUserBoxesAsync(userId)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransferViewModel model)
        {
            var userId = _userManager.GetUserId(User)!;

            if (!ModelState.IsValid)
            {
                model.Boxes = await _boxService.GetUserBoxesAsync(userId);
                return View(model);
            }

            var (success, message) = await _transferService.CreateAsync(
                userId, model.FromBoxId, model.ToBoxId, model.Amount, model.Date, model.Description);

            if (!success)
            {
                ModelState.AddModelError("", message);
                model.Boxes = await _boxService.GetUserBoxesAsync(userId);
                return View(model);
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}