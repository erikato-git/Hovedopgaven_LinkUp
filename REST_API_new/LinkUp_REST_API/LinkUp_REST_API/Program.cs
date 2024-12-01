using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGenWithJWTAuth();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hovedopgave;Integrated Security=SSPI;Trust Server Certificate=True");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }    // make it public for WebApplicationFactory in REST_API_TESTS