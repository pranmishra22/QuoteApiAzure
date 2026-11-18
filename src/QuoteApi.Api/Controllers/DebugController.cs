using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteApi.Infra;

namespace QuoteApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DebugController : ControllerBase
{
    private readonly QuoteDbContext _db;
    private readonly ILogger<QuotesController> _logger;
    private readonly IConfiguration _config;

    public DebugController(QuoteDbContext db, ILogger<QuotesController> logger, IConfiguration config) { 
        _db = db;
        _logger = logger;
        _config = config;
    }

    [HttpGet("slow")]
    public async Task<IActionResult> Slow()
    {
        if (!_config.GetValue<bool>("EnableDebugEndpoints")) return NotFound();

        _logger.LogInformation("Starting slow lookup");
        await _db.Database.ExecuteSqlRawAsync("WAITFOR DELAY '00:00:05'");
        _logger.LogInformation("Slow lookup finished");
        return Ok(new { delayed = true });
    }
}