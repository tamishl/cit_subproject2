// See https://aka.ms/new-console-template for more information

using DataServiceLayer.Services;

string password = Environment.GetEnvironmentVariable("PG_PASSWORD");
Console.WriteLine($" this is my password: {password}");

var titleService = new TitleService();

Console.WriteLine(titleService.GetTitleNameById("tt10691922"));

Console.WriteLine(titleService.GetTitlesByName("Potter")[1].PrimaryTitle);


