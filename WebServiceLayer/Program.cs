
// Starting point for configuring web application
using DataServiceLayer.Services;
using DataServiceLayer.Services.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebServiceLayer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<ITitleService, TitleService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton(new Hashing());

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

builder.Services.AddHttpContextAccessor();

// Register Mapper service for dependency injection
builder.Services.AddScoped<WebServiceLayer.Services.Mapper>();
builder.Services.AddMapster();

// Create the app
var app = builder.Build();

// Configure the HTTP request pipeline: sequence of components that each HTTP request passes through
app.UseAuthorization();

// Map incoming HTTP requests to controller actions through routing/endpoints
app.MapControllers();

// Start the server to listen for incoming HTTP requests
app.Run();




// optional code that was added automatically
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();
