using FinancialBox.Models;

namespace FinancialBox.Services.Interfaces
{
    public interface IBoxService
    {
        Task<List<Box>> GetUserBoxesAsync(string userId);
        Task<Box?> GetByIdAsync(int id, string userId);
        Task<Box> CreateAsync(string userId, string name, string currency, decimal initialBalance);
      Task<(bool Success, string Message)> UpdateAsync(int id, string userId, string name, string currency);
      Task<(bool Success, string Message)> DeleteAsync(int id, string userId);  
    }
}