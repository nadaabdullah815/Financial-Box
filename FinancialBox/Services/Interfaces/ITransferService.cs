using FinancialBox.Models;

namespace FinancialBox.Services.Interfaces
{
    public interface ITransferService
    {
        Task<List<Transfer>> GetUserTransfersAsync(string userId);
        Task<(bool Success, string Message)> CreateAsync(
            string userId, int fromBoxId, int toBoxId, decimal amount, DateTime date, string? description);
    }
}