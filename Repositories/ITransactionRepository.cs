using FinanceTrackerAPI.Models;

namespace FinanceTrackerAPI.Repositories;

// ── Interface ─────────────────────────────────────────────

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllByUserAsync(int userId);
    Task<Transaction?> GetByIdAsync(int id, int userId);
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<Transaction?> UpdateAsync(Transaction transaction);
    Task<bool> DeleteAsync(int id, int userId);
}
