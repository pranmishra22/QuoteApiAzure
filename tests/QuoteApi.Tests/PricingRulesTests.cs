using QuoteApi.Domain;

namespace QuoteApi.Tests;

public class PricingRulesTests
{
    [TestCase(100000, 30, 2000)]
    [TestCase(100000, 20, 2100)]
    public void Calculate_returns_expected_premium(decimal cover, int age, decimal expected)
    {
        Assert.That(PricingRules.Calculate(cover, age), Is.EqualTo(expected));
    }
}