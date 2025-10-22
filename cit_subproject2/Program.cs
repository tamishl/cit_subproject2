// See https://aka.ms/new-console-template for more information
string password = Environment.GetEnvironmentVariable("PG_PASSWORD");
Console.WriteLine($" this is my password: {password}");
