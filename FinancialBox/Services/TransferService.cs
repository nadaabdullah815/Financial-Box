using FinancialBox.Data;
using FinancialBox.Models;
using FinancialBox.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Services
{
    public class TransferService : ITransferService
    {
        private readonly AppDbContext _context;

        public TransferService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transfer>> GetUserTransfersAsync(string userId)
        {
            return await _context.Transfers
                .Include(t => t.FromBox)
                .Include(t => t.ToBox)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> CreateAsync(
            string userId, int fromBoxId, int toBoxId, decimal amount, DateTime date, string? description)
        {
            if (amount <= 0)
                return (false, "المبلغ يجب أن يكون أكبر من صفر");

            if (fromBoxId == toBoxId)
                return (false, "لا يمكن التحويل إلى نفس الصندوق");

            // الصندوقان لازم يكونوا للمستخدم نفسه
            var fromBox = await _context.FinancialBoxes
                .FirstOrDefaultAsync(b => b.Id == fromBoxId && b.UserId == userId);
            var toBox = await _context.FinancialBoxes
                .FirstOrDefaultAsync(b => b.Id == toBoxId && b.UserId == userId);

            if (fromBox == null || toBox == null)
                return (false, "أحد الصندوقين غير موجود أو لا يخصك");

            if (fromBox.Currency != toBox.Currency)
                return (false, "لا يمكن التحويل بين صندوقين بعملتين مختلفتين");

            if (fromBox.Balance < amount)
                return (false, "الرصيد غير كافٍ في الصندوق المحوَّل منه");

            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                fromBox.Balance -= amount;
                toBox.Balance += amount;

                _context.Transfers.Add(new Transfer
                {
                    FromBoxId = fromBoxId,
                    ToBoxId = toBoxId,
                    Amount = amount,
                    Date = date,
                    Description = description?.Trim(),
                    UserId = userId
                });

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
                return (true, "تمت الحوالة بنجاح");
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                return (false, "حدث خطأ أثناء تنفيذ الحوالة، يرجى المحاولة مرة أخرى");
            }
        }
    }
}