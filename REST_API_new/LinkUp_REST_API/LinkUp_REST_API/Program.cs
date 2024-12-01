using LinkUp_REST_API.Core;
using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Extensions;
using LinkUp_REST_API.Repositories;
using LinkUp_REST_API.Repositories.Interfaces;
using LinkUp_REST_API.Services;
using LinkUp_REST_API.Services.Interfaces;
using LinkUp_REST_API.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();


/*
 * Need to use Options-pattern, otherwise if I use Configuration it will affect all strings in the application in .NET 9
 */
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<JwtAuthenticationService>();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGenWithJWTAuth();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();     // add 'swagger' at the end of the URL
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }    // make it public for WebApplicationFactory in REST_API_TESTS