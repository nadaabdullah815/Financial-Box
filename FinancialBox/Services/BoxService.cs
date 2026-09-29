using FinancialBox.Models;
using FinancialBox.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Services
{
    public class BoxService : IBoxService
    {
        private readonly AppDbContext _context;

        public BoxService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Box>> GetUserBoxesAsync(string userId)
        {
            return await _context.FinancialBoxes
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<Box?> GetByIdAsync(int id, string userId)
        {
            return await _context.FinancialBoxes
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
        }

        public async Task<Box> CreateAsync(string userId, string name, string currency, decimal initialBalance)
        {
            var box = new Box
            {
                Name = name,
                Currency = currency,
                Balance = initialBalance,
                UserId = userId
            };

            _context.FinancialBoxes.Add(box);
            await _context.SaveChangesAsync();
            return box;
        }

        public async Task<bool> UpdateAsync(int id, string userId, string name, string currency)
        {
            var box = await GetByIdAsync(id, userId);
            if (box == null) return false;

            box.Name = name;
           // box.Currency = currency;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id, string userId)
        {
            var box = await GetByIdAsync(id, userId);
            if (box == null) return (false, "الصندوق غير موجود");
        
            if (box.IsDefault)
                return (false, "لا يمكن حذف هذا الصندوق لأنه صندوق أساسي بالنظام");
            
            bool hasTransfers = await _context.Transfers
                 .AnyAsync(t => t.FromBoxId == id || t.ToBoxId == id);
          
            if(hasTransfers)
                 return (false, "لا يمكن حذف هذا الصندوق لوجود حوالات مرتبطة به");

            _context.FinancialBoxes.Remove(box);
            await _context.SaveChangesAsync();
            return (true, "تم حذف الصندوق بنجاح");
        }
    }
}