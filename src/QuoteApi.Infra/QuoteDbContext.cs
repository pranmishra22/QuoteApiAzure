using Microsoft.EntityFrameworkCore;
using QuoteApi.Domain;

namespace QuoteApi.Infra;
public class QuoteDbContext : DbContext
{
    public QuoteDbContext(DbContextOptions<QuoteDbContext> options) : base(options) { }

    public DbSet<Quote> Quotes => Set<Quote>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Quote>().Property(q => q.CoverAmount).HasPrecision(18, 2);
        b.Entity<Quote>().Property(q => q.Premium).HasPrecision(18, 2);
    }
}