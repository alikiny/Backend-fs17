using System.Text;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Service.src.Service;
using Ecommerce.Service.src.ServiceAbstraction;
using Ecommerce.WebApi.src.Data;
using Ecommerce.WebApi.src.middleware;
using Ecommerce.WebApi.src.Repo;
using Ecommerce.WebApi.src.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add all controllers
builder.Services.AddControllers();


builder.Services.AddDbContext<EcommerceDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PgDbConnection"));
});

// service registration -> automatically create all instances of dependencies
// user service registration
builder.Services.AddScoped<IUserRepository, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// product service registration
builder.Services.AddScoped<IProductRepository, ProductRepo>();
builder.Services.AddScoped<IProductService, ProductService>();

//category service registration
builder.Services.AddScoped<ICategoryRepository, CategoryRepo>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

//register ExceptionHandlerMiddleware
builder.Services.AddScoped<ExceptionHandlerMiddleware>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<ExceptionHandlerMiddleware>>();
    return new ExceptionHandlerMiddleware(next =>
    {
        var requestDelegate = serviceProvider.GetRequiredService<RequestDelegate>();
        return Task.CompletedTask;
    }, logger);
});

// Add authentication instructions
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Secrets:JwtKey"])
            ),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Secrets:Issuer"]
        };
    });

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure logging
builder.Services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddConsole();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication(); // extract auth header and validate it
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
