using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using DataServiceLayer.Services;
using DataServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using WebServiceLayer.DTOs;
using WebServiceLayer.Services;


namespace WebServiceLayer.Controllers;

[Route("api/users/{username}/bookmarks")]
[ApiController]

public class BookmarkController : BaseController
{
    private IBookmarkService _bookmarkService;
    private Mapper _mapper;

    public BookmarkController(IBookmarkService bookmarkService, LinkGenerator linkGenerator, Mapper mapper)
    : base(linkGenerator)
    {

        _bookmarkService = bookmarkService;
        _mapper = mapper;
    }


    // TITLE BOOKMARKS //

    [HttpPost("titles/{titleId}", Name = nameof(CreateBookmarkTitle))]
    public IActionResult CreateBookmarkTitle(string username, string titleId)
    {
        try
        {
            var bookmark = _bookmarkService.CreateBookmarkTitle(titleId, username, null);

            var createdBookmarkDto = _mapper.CreateBookmarkTitleDto(bookmark);

            return Created(createdBookmarkDto.BookmarkUrl, createdBookmarkDto);

        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("titles", Name = nameof(GetBookmarkedTitlesUser))]

    public IActionResult GetBookmarkedTitlesUser(string username, [FromQuery] PageSettings pageSettings)
    {

        var bookmarks = _bookmarkService.GetBookmarkedTitles(username, pageSettings.Page, pageSettings.PageSize);

        if (bookmarks.TotalNumberOfItems == 0)
        {
            return NotFound("No bookmarks found");
        }


        var bookmarkDto = bookmarks.Items?.Select(b => _mapper.BookmarkTitleDto(b)).ToList();

        var result = CreatePaging(nameof(GetBookmarkedTitlesUser), bookmarkDto, bookmarks.TotalNumberOfItems.Value, pageSettings);

        return Ok(result);

    }

    [HttpDelete("titles/{titleId}")]

    public IActionResult DeleteBookmarkTitle(string username, string titleId)
    {
        var deletedBookmark = _bookmarkService.DeleteBookmarkTitle(titleId, username);

        if (deletedBookmark == null)
        {
            return NotFound("No bookmark found");
        }

        return Ok(deletedBookmark);
    }


    [HttpPut("titles/{titleId}")]

    public IActionResult UpdateBookmarkTitle(string username, string titleId, string note = null)
    {
        var bookmarkedUpdated = _bookmarkService.UpdateBookmarkTitle(titleId, username, note);

        if (bookmarkedUpdated)
        {
            var updatedBookmark = _bookmarkService.GetBookmarkedTitle(username, titleId);

            return Ok(updatedBookmark);
        }

        return NotFound("No bookmark found");
    }


    // PERSON BOOKMARKS //

    [HttpPost("persons/{personId}", Name = nameof(CreateBookmarkPerson))]

    public IActionResult CreateBookmarkPerson(string username, string personId, string note = null)
    {
        try
        {
            var bookmark = _bookmarkService.CreateBookmarkPerson(personId, username, note);

            var createdBookmarkDto = _mapper.CreateBookmarkPersonDto(bookmark);

            return Created(createdBookmarkDto.BookmarkUrl, createdBookmarkDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("persons", Name = nameof(GetBookmarkedPersonsUser))]

    public IActionResult GetBookmarkedPersonsUser(string username, PageSettings pagesettings)
    {
        var bookmarks = _bookmarkService.GetBookmarkPersons(username, pagesettings.Page, pagesettings.PageSize);

        if (bookmarks == null)
        {
            return NotFound("no bookmarks for persons)");
        }

        var bookmarkDto = bookmarks.Items?.Select(bp => _mapper.BookmarkPersonDto(bp)).ToList();

        var result = CreatePaging(nameof(GetBookmarkedPersonsUser), bookmarks.Items, bookmarks.TotalNumberOfItems.Value, pagesettings);

        return Ok(result);
    }

    [HttpDelete("persons/{personId}")]

    public IActionResult UpdateBookmarkPerson(string username, string personId, string note = null)
    {
        var bookmarkUpdated = _bookmarkService.UpdateBookmarkPerson(username, personId, note);

        if (bookmarkUpdated)
        {
            var updatedBookmark = _bookmarkService.GetBookmarkedPerson(username, personId);
            return Ok(updatedBookmark);
        }

        return NotFound("Bookmark not found");
    }

    [HttpPut("persons/{personId}")]

    public IActionResult DeleteBookmarkPerson(string username, string personId)
    {
        var deletedBookmark = _bookmarkService.DeleteBookmarkPerson(personId, username);

        if (deletedBookmark == null)
        {
            return NotFound("Bookmark not Found");
        }

        return Ok(deletedBookmark);
    }

}

