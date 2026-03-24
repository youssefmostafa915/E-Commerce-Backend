using E_Commerce.Application.Extension;
using E_Commerce.Infrastructure.Extension;
using E_Commerce.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Register our Custom Layers (The Brain and the Muscle)
// This fixes the "Unable to resolve service" error
builder.Services.AddInfrastructure(builder.Configuration); 
builder.Services.AddApplication();

// 2. Configure JWT Authentication
// This enables the [Authorize] attribute on controllers
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(options =>
{
    // Set JWT as the default authentication scheme
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure JWT bearer token validation
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Verify token was issued by trusted source
        ValidateAudience = true, // Verify token is for our application
        ValidateLifetime = true, // Check if token is expired
        ValidateIssuerSigningKey = true, // Verify signature with our secret key
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero // No tolerance for expired tokens
    };
});

// 3. Register Standard Web API Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adds the UI to test your API in the browser

// Register MediatR for command/query dispatching
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// 4. Configure the HTTP Request Pipeline (The "Rules of the Road")
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Important: This allows the API to redirect HTTP to HTTPS
// If you have issues with SSL, you can comment this out for local testing
app.UseHttpsRedirection();

// Add global exception handling middleware (must be first)
app.UseGlobalExceptionHandler();

// Enable authentication middleware (must come before authorization)
app.UseAuthentication();

// Enable authorization middleware
app.UseAuthorization();

// 5. Map the Controller Routes
// This is what makes 'api/products' actually work
app.MapControllers();

app.Run();