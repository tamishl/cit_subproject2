
// Starting point for configuring web application
using DataServiceLayer.Services;
using DataServiceLayer.Services.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebServiceLayer.Services;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // name policy for Cross-Origin Resource Sharing

var builder = WebApplication.CreateBuilder(args);




// Add services to the container
builder.Services.AddScoped<ITitleService, TitleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddSingleton(new Hashing());

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });

});




// Authentication
var secret = builder.Configuration.GetSection("Auth:Secret").Value;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        }
    );

builder.Services.AddControllers();

// Make HttpContext accessible
builder.Services.AddHttpContextAccessor();

// Register Mapper service for dependency injection
builder.Services.AddScoped<WebServiceLayer.Services.Mapper>();
builder.Services.AddMapster();

// Create the app
var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins); // add CORS middleware

// Note: UseCors must be placed before UseAuthorization (request has to be run before authentication).
// Configure the HTTP request pipeline: sequence of components that each HTTP request passes through
app.UseAuthorization();

// Map incoming HTTP requests to controller actions through routing/endpoints
app.MapControllers();

// Start the server to listen for incoming HTTP requests
app.Run();