using System.ComponentModel.DataAnnotations;

namespace QuoteApi.Api.Dtos;

public class CreateQuoteRequest
{
    [Range(1, 10_000_000)] public decimal CoverAmount { get; init; }
    [Range(18, 100)] public int CustomerAge { get; init; }
}

public record QuoteResponse(Guid Id, string Status, decimal CoverAmount, decimal Premium, DateTime CreatedAt);