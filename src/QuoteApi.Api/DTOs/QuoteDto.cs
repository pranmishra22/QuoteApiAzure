using System.ComponentModel.DataAnnotations;

namespace QuoteApi.Api.Dtos;

public class CreateQuoteRequest
{
    /// <summary>Amount to insure, between 1 and 10,000,000.</summary>
    [Range(1, 10_000_000)] public decimal CoverAmount { get; init; }

    /// <summary>Customer age in years, 18 to 100.</summary>
    [Range(18, 100)] public int CustomerAge { get; init; }
}

public record QuoteResponse(Guid Id, string Status, decimal CoverAmount, decimal Premium, DateTime CreatedAt);