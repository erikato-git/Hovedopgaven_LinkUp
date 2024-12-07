using LinkUp_REST_API.Core;
using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Extensions;
using LinkUp_REST_API.Repositories.Completed;
using LinkUp_REST_API.Repositories.Interfaces;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using LinkUp_REST_API.Services.Completed;
using LinkUp_REST_API.Services.Interfaces;
using LinkUp_REST_API.Services.Interfaces.Completed;
using LinkUp_REST_API.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAuthentication, Authentication>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileServiceHelper, ProfileServiceHelper>();

builder.Services.AddScoped<IPitchRepository, PitchRepository>();
builder.Services.AddScoped<IPitchService, PitchService>();

builder.Services.AddScoped<IKeywordRepository, KeywordRepository>();
builder.Services.AddScoped<IKeywordService, KeywordService>();



builder.Services.AddDbContext<DataContext>();

// JWT
//builder.Services.AddScoped<JwtAuthenticationService>();

// JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)),
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            ClockSkew = TimeSpan.Zero,
        };
    });



// Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));     // IOptions-pattern

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGenWithJWTAuth();        // TODO: Find out why authentication is not working when I inject JWT-string


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();     // add 'swagger' at the end of the URL
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }    // make it public for WebApplicationFactory in REST_API_TESTS