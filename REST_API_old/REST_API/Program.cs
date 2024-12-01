using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using REST_API.Controllers.IHelpers;
using REST_API.Data;
using REST_API.Repositories;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Domains;
using REST_API.Services.Helpers;
using REST_API.Services.IHelpers;
using REST_API.Services.Interfaces;
using REST_API.Util;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAccountRepository,AccountRepository>();
builder.Services.AddScoped<IAccountService,AccountService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IPitchRepository, PitchRepository>();
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IPhotoAccessor,PhotoAccessor>();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));     // IOptions-pattern


/*
 * Reference: https://www.youtube.com/watch?v=6DWJIyipxzw
 */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;     // TODO: Check if I should remove this part
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
            ValidIssuer = builder.Configuration["JWT:Issuers"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            ClockSkew = TimeSpan.Zero,
        };
    });

// Somehow Swagger adds authentication to all endpoints when I configure it with JWT-authentication and I don't know how to configure Swagger to respect AllowAnonymous for endpoints
// TODO: Try another tool than Swagger e.g. Postman
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Manage which database-settings I wanna apply for the system
builder.Services.AddDbContext<MssqlDbContext>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }    // make it public for WebApplicationFactory in REST_API_TESTS