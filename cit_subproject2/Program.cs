// See https://aka.ms/new-console-template for more information

using DataServiceLayer.Services;

string password = Environment.GetEnvironmentVariable("PG_PASSWORD");
Console.WriteLine($" this is my password: {password}");

var titleService = new TitleService();

Console.WriteLine(titleService.GetTitleNameById("tt10691922"));

Console.WriteLine(titleService.GetTitlesByName("Potter")[1].PrimaryTitle);


// test integer division rounding up
int a = 5;
int b = 2;

Console.WriteLine((a + b - 1) / b);


