// See https://aka.ms/new-console-template for more information

using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using DataServiceLayer.Services;
using System.Text.Json;

string password = Environment.GetEnvironmentVariable("PG_PASSWORD");
Console.WriteLine($" this is my password: {password}");

var titleService = new TitleService();
var bookmarkService = new BookmarkService();
var userService = new UserService();

Console.WriteLine(titleService.GetTitle("tt0081912"));

//Console.WriteLine(titleService.GetTitlesByGenre("Fantasy"));
//Console.WriteLine(titleService.GetTitlesByType("movie"));

//Console.WriteLine(titleService.GetTitlesByName("Potter").Items[1].PrimaryTitle);

//userService.DeleteUser(username: "Blommo");
//userService.CreateUser(username: "Blommo",
//                                             email: "minMail@hotmail.com",
//                                             password: "etpassword",
//                                             salt: "saltmedmeresalt");

////Create
//bookmarkService.CreateBookmarkTitle("tt33042905", "Blommo", "KÆÆÆFt den er fed!");

////Get

//var result = bookmarkService.GetBookmarkedTitles("Blommo");

//string json = JsonSerializer.Serialize(result, new JsonSerializerOptions
//{
//    WriteIndented = true
//});

//Console.WriteLine(json);


//userService.DeleteUser(username: "Blommo");
/*



public void UpdateBookmarkTitle(string titleId, string username, string note);
public BookmarkPerson CreateBookmarkPerson(string personId, string username, string note);
public PagedResultDto<BookmarkPersonDto> GetAllPersonBookmarks();
public PagedResultDto<BookmarkPersonDto> GetBookmarkPersons(string username);
public void DeleteBookmarkPerson(string personId, string username);
public void UpdateBookmarkPerson(string personId, string username, string note);

*/
// test integer division rounding up
//int a = 5;
//int b = 2;

//Console.WriteLine((a + b - 1) / b);


