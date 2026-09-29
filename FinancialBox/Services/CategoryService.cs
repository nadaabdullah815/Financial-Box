using FinancialBox.Data;
using FinancialBox.Models;
using FinancialBox.Models.Enum;
using FinancialBox.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetUserCategoriesAsync(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == null || c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id, string userId)
        {
            // بالتعديل/الحذف، بس التصنيف الخاص بالمستخدم مسموح (UserId == userId بس، مش null)
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

       public async Task<(bool Success, string Message)> CreateAsync(string userId, string name, CategoryType type)
     {
         // التحقق من التكرار: بين التصنيفات العامة + الخاصة بالمستخدم، بنفس النوع
         bool exists = await _context.Categories.AnyAsync(c =>
             (c.UserId == null || c.UserId == userId) &&
             c.Type == type &&
             c.Name.Trim().ToLower() == name.Trim().ToLower());
     
         if (exists)
             return (false, "يوجد تصنيف بنفس الاسم والنوع مسبقًا");
     
         var category = new Category { Name = name.Trim(), Type = type, UserId = userId };
         _context.Categories.Add(category);
         await _context.SaveChangesAsync();
         return (true, "تمت الإضافة بنجاح");
     }

     
public async Task<(bool Success, string Message)> UpdateAsync(int id, string userId, string name, CategoryType type)
{
    var category = await GetByIdAsync(id, userId);
    if (category == null) return (false, "التصنيف غير موجود");

    // التحقق من التكرار، باستثناء التصنيف الحالي نفسه (عشان تقدري تعدّلي بدون تغيير الاسم)
    bool exists = await _context.Categories.AnyAsync(c =>
        c.Id != id &&
        (c.UserId == null || c.UserId == userId) &&
        c.Type == type &&
        c.Name.Trim().ToLower() == name.Trim().ToLower());

    if (exists)
        return (false, "يوجد تصنيف بنفس الاسم والنوع مسبقًا");

    category.Name = name.Trim();
    category.Type = type;
    await _context.SaveChangesAsync();
    return (true, "تم التعديل بنجاح");
}

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var category = await GetByIdAsync(id, userId);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}