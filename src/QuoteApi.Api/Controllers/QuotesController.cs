using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteApi.Api.Dtos;
using QuoteApi.Domain;
using QuoteApi.Infra;

namespace QuoteApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class QuotesController : ControllerBase
{
    private readonly QuoteDbContext _db;
    private readonly ILogger<QuotesController> _logger;
    public QuotesController(QuoteDbContext db, ILogger<QuotesController> logger) { 
        _db = db;
        _logger = logger;
    }

    /// <summary>Creates a new insurance quote.</summary>
    /// <param name="req">Cover amount and customer age.</param>
    /// <response code="201">Quote created.</response>
    /// <response code="400">Invalid input.</response>
    [HttpPost]
    [ProducesResponseType(typeof(QuoteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuoteResponse>> Create(CreateQuoteRequest req)
    {
        var quote = new Quote
        {
            CoverAmount = req.CoverAmount,
            CustomerAge = req.CustomerAge,
            Premium = PricingRules.Calculate(req.CoverAmount, req.CustomerAge)
        };
        _db.Quotes.Add(quote);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Quote created {QuoteId} with premium {Premium}", quote.Id, quote.Premium);
        return CreatedAtAction(nameof(Get), new { id = quote.Id }, ToResponse(quote));
    }

    /// <summary>Gets a quote by ID.</summary>
    /// <param name="id">Quote ID.</param>
    /// <response code="200">Quote found with the ID.</response>
    /// <response code="404">No quote with that ID.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(QuoteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuoteResponse>> Get(Guid id)
    {
        var quote = await _db.Quotes.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
        return quote is null ? NotFound() : ToResponse(quote);
    }

    private static QuoteResponse ToResponse(Quote q) =>
        new(q.Id, q.Status, q.CoverAmount, q.Premium, q.CreatedAt);
}