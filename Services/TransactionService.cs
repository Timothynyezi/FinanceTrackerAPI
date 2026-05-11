using FinanceTrackerAPI.DTOs;
using FinanceTrackerAPI.Models;
using FinanceTrackerAPI.Repositories;

namespace FinanceTrackerAPI.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionResponseDto>> GetAllAsync(int userId);
    Task<TransactionResponseDto?> GetByIdAsync(int id, int userId);
    Task<TransactionResponseDto> CreateAsync(CreateTransactionDto dto, int userId);
    Task<TransactionResponseDto?> UpdateAsync(int id, UpdateTransactionDto dto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repo;

    public TransactionService(ITransactionRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<TransactionResponseDto>> GetAllAsync(int userId string? type = null)
    {
        var transactions = await _repo.GetAllByUserAsync(userId);

        if (!string.IsNullOrEmpty(type))
        {
            transactions = transactions
                .Where(t => t.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        }
        return transactions.Select(MapToDto);
    }

    public async Task<TransactionResponseDto?> GetByIdAsync(int id, int userId)
    {
        var transaction = await _repo.GetByIdAsync(id, userId);
        return transaction is null ? null : MapToDto(transaction);
    }

    public async Task<TransactionResponseDto> CreateAsync(CreateTransactionDto dto, int userId)
    {
        var transaction = new Transaction
        {
            Description = dto.Description,
            Amount = dto.Amount,
            Date = dto.Date,
            Type = dto.Type,
            CategoryId = dto.CategoryId,
            UserId = userId
        };

        var created = await _repo.CreateAsync(transaction);
        // Reload with category included
        var result = await _repo.GetByIdAsync(created.Id, userId);
        return MapToDto(result!);
    }

    public async Task<TransactionResponseDto?> UpdateAsync(int id, UpdateTransactionDto dto, int userId)
    {
        var existing = await _repo.GetByIdAsync(id, userId);
        if (existing is null) return null;

        if (dto.Description is not null) existing.Description = dto.Description;
        if (dto.Amount.HasValue) existing.Amount = dto.Amount.Value;
        if (dto.Date.HasValue) existing.Date = dto.Date.Value;
        if (dto.Type.HasValue) existing.Type = dto.Type.Value;
        if (dto.CategoryId.HasValue) existing.CategoryId = dto.CategoryId.Value;

        await _repo.UpdateAsync(existing);
        var updated = await _repo.GetByIdAsync(id, userId);
        return MapToDto(updated!);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        return await _repo.DeleteAsync(id, userId);
    }

    private static TransactionResponseDto MapToDto(Transaction t) => new()
    {
        Id = t.Id,
        Description = t.Description,
        Amount = t.Amount,
        Date = t.Date,
        Type = t.Type.ToString(),
        Category = t.Category?.Name ?? "Unknown"
    };
}
