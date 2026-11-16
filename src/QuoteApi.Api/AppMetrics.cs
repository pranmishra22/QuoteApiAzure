using System.Diagnostics.Metrics;

namespace QuoteApi.Api;

public static class AppMetrics
{
    public const string MeterName = "QuoteApi";
    public static readonly Meter Meter = new(MeterName);
    public static readonly Counter<long> QuotesCreated = Meter.CreateCounter<long>("quotes_created_total");
}