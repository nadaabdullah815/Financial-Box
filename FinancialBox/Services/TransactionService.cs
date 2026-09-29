using FinancialBox.Models;
using FinancialBox.Models.Enum;
using FinancialBox.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace FinancialBox.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetBoxTransactionsAsync(int boxId, string userId)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.BoxId == boxId && t.Box.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> AddTransactionAsync(
            string userId, int boxId, int categoryId, decimal amount, CategoryType type, string? description, DateTime date)
        {
            // 1. قواعد الـ Validation الأساسية
            if (amount <= 0)
                return (false, "المبلغ يجب أن يكون أكبر من صفر");

            var box = await _context.FinancialBoxes
                .FirstOrDefaultAsync(b => b.Id == boxId && b.UserId == userId);

            if (box == null)
                return (false, "الصندوق غير موجود أو لا يخصك");

            var category = await _context.Categories
              .FirstOrDefaultAsync(c => c.Id == categoryId && (c.UserId == null || c.UserId == userId));
            if (category == null)
                return (false, "التصنيف غير موجود");
            
            if (category.Type != type)
                return (false, "نوع التصنيف لا يطابق نوع العملية");

            // 2. التحقق من كفاية الرصيد (بحالة المصروف فقط)
            if (type == CategoryType.Expense && box.Balance < amount)
                return (false, "الرصيد غير كافٍ لإتمام هذه العملية");

            // 3. تنفيذ العملية داخل Transaction موحدة بقاعدة البيانات
            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var transaction = new Transaction
                {
                    Amount = amount,
                    Date = date,
                    Description = description,
                    BoxId = boxId,
                    CategoryId = categoryId
                };

                _context.Transactions.Add(transaction);

                // تحديث الرصيد حسب نوع العملية
                if (type == CategoryType.Income)
                    box.Balance += amount;
                else
                    box.Balance -= amount;

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return (true, "تمت العملية بنجاح");
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                return (false, "حدث خطأ أثناء تنفيذ العملية، حاولي مرة أخرى");
            }
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId, string userId)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Box)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.Box.UserId == userId);

            if (transaction == null) return false;

            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // عكس تأثير العملية على الرصيد قبل الحذف
                if (transaction.Category.Type == CategoryType.Income)
                    transaction.Box.Balance -= transaction.Amount;
                else
                    transaction.Box.Balance += transaction.Amount;

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
                return true;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                return false;
            }
        }
    }
}