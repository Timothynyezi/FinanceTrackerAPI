namespace FinanceTrackerAPI.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
