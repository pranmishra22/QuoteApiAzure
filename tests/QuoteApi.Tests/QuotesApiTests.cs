using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using QuoteApi.Api.Dtos;
using QuoteApi.Infra;

namespace QuoteApi.Tests;

public class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<QuoteDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<QuoteDbContext>>();
            services.AddDbContext<QuoteDbContext>(o => o.UseInMemoryDatabase("tests"));
        });
    }
}

public class QuotesApiTests
{
    private ApiFactory _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _factory = new ApiFactory();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void Teardown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task Post_then_get_returns_the_same_quote()
    {
        var create = await _client.PostAsJsonAsync("/quotes", new { coverAmount = 100000, customerAge = 30 });
        Assert.That(create.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var created = await create.Content.ReadFromJsonAsync<QuoteResponse>();
        var get = await _client.GetAsync($"/quotes/{created!.Id}");
        Assert.That(get.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var fetched = await get.Content.ReadFromJsonAsync<QuoteResponse>();
        Assert.That(fetched!.Premium, Is.EqualTo(2000m));
    }

    [Test]
    public async Task Post_with_invalid_age_returns_400()
    {
        var res = await _client.PostAsJsonAsync("/quotes", new { coverAmount = 100000, customerAge = 5 });
        Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Get_unknown_id_returns_404()
    {
        var res = await _client.GetAsync($"/quotes/{Guid.NewGuid()}");
        Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}