using FinancialBox.ViewModels;

namespace FinancialBox.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportViewModel> GetReportAsync(string userId, DateTime from, DateTime to);
    }
}