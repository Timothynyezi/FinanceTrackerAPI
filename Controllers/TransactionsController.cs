using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinanceTrackerAPI.DTOs;
using FinanceTrackerAPI.Services;

namespace FinanceTrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionsController(ITransactionService service)
    {
        _service = service;
    }

    // Helper to extract the logged-in user's ID from JWT claims
    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET api/transactions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transactions = await _service.GetAllAsync(GetUserId());
        return Ok(transactions);
    }

    // GET api/transactions/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var transaction = await _service.GetByIdAsync(id, GetUserId());
        if (transaction is null) return NotFound(new { message = "Transaction not found" });
        return Ok(transaction);
    }

    // POST api/transactions
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto, GetUserId());
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT api/transactions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTransactionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto, GetUserId());
        if (updated is null) return NotFound(new { message = "Transaction not found" });
        return Ok(updated);
    }

    // DELETE api/transactions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id, GetUserId());
        if (!deleted) return NotFound(new { message = "Transaction not found" });
        return NoContent();
    }
    // Get api/transactions/summery
    [HttpGet("summery")]
    public async Task<IActionResult> GetSummary()
    {
        var transactions = await _service.GetAllAsync(GetUserId());

        var totalIncome = transactions
            .Where(t => t.Type == "Income")
            .Sum(t => t.Amount);

        var totalExpenses = transactions
            .Where(t => t.Type == "Expense")
            .Sum(t => t.Amount);
        
        var netBalance = totalIncome - totalExpenses;

        return Ok(new
        {
            totalIncome,
            totalExpenses,
            netBalance
        });
    }
}
