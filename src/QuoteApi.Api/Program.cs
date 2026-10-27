using Microsoft.EntityFrameworkCore;
using QuoteApi.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<QuoteDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();                          // /openapi/v1.json
    app.MapOpenApi("/openapi/{documentName}.yaml");  // /openapi/v1.yaml
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "QuoteApi v1");
    });
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<QuoteDbContext>().Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public partial class Program { }
