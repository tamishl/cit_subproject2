
// Starting point for configuring web application
using DataServiceLayer.Services;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<ITitleService, TitleService>();

builder.Services.AddControllers();

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
