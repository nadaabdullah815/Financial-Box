using FinancialBox.Models;
using FinancialBox.Models.Enum;

namespace FinancialBox.Data
{
    public static class SeedData
    {
        public static async Task SeedCategoriesAsync(AppDbContext context)
        {
            if (context.Categories.Any(c => c.UserId == null)) return;

            var categories = new List<Category>
            {
                // إيرادات
                new Category { Name = "راتب", Type = CategoryType.Income, UserId = null },
                new Category { Name = "عمل حر", Type = CategoryType.Income, UserId = null },
                new Category { Name = "استثمار", Type = CategoryType.Income, UserId = null },
                new Category { Name = "إضافي", Type = CategoryType.Income, UserId = null },
                new Category { Name = "أخرى", Type = CategoryType.Income, UserId = null },

                // مصاريف
                new Category { Name = "طعام", Type = CategoryType.Expense, UserId = null },
                new Category { Name = "أدوات منزلية", Type = CategoryType.Expense, UserId = null },
                new Category { Name = "فاتورة", Type = CategoryType.Expense, UserId = null },
                new Category { Name = "مواصلات", Type = CategoryType.Expense, UserId = null },
                new Category { Name = "أخرى", Type = CategoryType.Expense, UserId = null },
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        // 👈 دالة جديدة: تُستدعى لكل مستخدم جديد وقت التسجيل
        public static async Task SeedDefaultBoxesAsync(AppDbContext context, string userId)
        {
            var defaultBoxes = new List<Box>
            {
                new Box { Name = "شام كاش", Currency = "SYP", Balance = 0,IsDefault = true, UserId = userId },
                new Box { Name = "سيرياتيل كاش", Currency = "SYP", Balance = 0,IsDefault = true, UserId = userId },
                new Box { Name = "النقدي للمنزل", Currency = "SYP", Balance = 0,IsDefault = true, UserId = userId },
                new Box { Name = "تسوية الديون", Currency = "SYP", Balance = 0,IsDefault = true, TargetAmount = null, UserId = userId },
            };

            context.FinancialBoxes.AddRange(defaultBoxes);
            await context.SaveChangesAsync();
        }
    }
}