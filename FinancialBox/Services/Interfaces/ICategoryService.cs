using FinancialBox.Models;
using FinancialBox.Models.Enum;
using FinancialBox.Services.Interfaces;

namespace FinancialBox.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetUserCategoriesAsync(string userId); // عامة + خاصة بالمستخدم
        Task<Category?> GetByIdAsync(int id, string userId);
       Task<(bool Success, string Message)> CreateAsync(string userId, string name, CategoryType type);   
    Task<(bool Success, string Message)> UpdateAsync(int id, string userId, string name, CategoryType type);  
        Task<bool> DeleteAsync(int id, string userId);
    }
}