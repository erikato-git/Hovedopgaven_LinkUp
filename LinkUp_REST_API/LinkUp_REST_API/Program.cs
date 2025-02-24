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


// Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));     // IOptions-pattern

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Extensions
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerGenWithJWTAuth();
builder.Services.AddCustomRateLimiter();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();     // add 'swagger' at the end of the URL
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();                  // It is recommended to use incognito mode when accessing the application in Chrome.


public partial class Program { }    // make it public for WebApplicationFactory in REST_API_TESTS