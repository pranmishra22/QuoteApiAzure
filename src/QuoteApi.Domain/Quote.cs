namespace QuoteApi.Domain;

public class Quote
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Status { get; set; } = "Created";
    public decimal CoverAmount { get; set; }
    public int CustomerAge { get; set; }
    public decimal Premium { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}

public static class PricingRules
{
    public static decimal Calculate(decimal coverAmount, int age) =>
        coverAmount * 0.02m + (age < 25 ? 100m : 0m);
}