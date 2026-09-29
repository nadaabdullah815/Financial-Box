using FinancialBox.Models;
using FinancialBox.Models.Enum;
using FinancialBox.Services;

namespace FinancialBox.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetBoxTransactionsAsync(int boxId, string userId);
        Task<(bool Success, string Message)> AddTransactionAsync(
            string userId, int boxId, int categoryId, decimal amount, CategoryType type, string? description, DateTime date);
        Task<bool> DeleteTransactionAsync(int transactionId, string userId);
    }
}