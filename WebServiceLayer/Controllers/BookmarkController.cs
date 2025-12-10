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

[Route("api/users/me/bookmarks")]
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
    [Authorize]
    public IActionResult CreateBookmarkTitle(string titleId, [FromBody] UpdateBookmarkDto? dto)
    {
        var username = HttpContext.User.Identity.Name;
        var note = dto?.Note;
        
        try
        {
            var bookmark = _bookmarkService.CreateBookmarkTitle(titleId, username, note);

            var createdBookmarkDto = _mapper.CreateBookmarkTitleDto(bookmark);

            return Created(createdBookmarkDto.BookmarkUrl, createdBookmarkDto);

        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("titles", Name = nameof(GetBookmarkedTitlesUser))]
    [Authorize]

    public IActionResult GetBookmarkedTitlesUser([FromQuery] PageSettings pageSettings)
    {
        var username = HttpContext.User.Identity.Name;

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
    [Authorize]

    public IActionResult DeleteBookmarkTitle(string titleId)
    {
        var username = HttpContext.User.Identity.Name;
        var deletedBookmark = _bookmarkService.DeleteBookmarkTitle(titleId, username);

        if (deletedBookmark == null)
        {
            return NotFound("No bookmark found");
        }

        return Ok("Bookmark deleted");
    }


    [HttpPut("titles/{titleId}")]
    [Authorize]

    public IActionResult UpdateBookmarkTitle(string titleId, [FromBody] UpdateBookmarkDto? dto)
    {
        var username = HttpContext.User.Identity.Name;
        var note = dto?.Note;
        var bookmarkedUpdated = _bookmarkService.UpdateBookmarkTitle(titleId, username, note);

        if (bookmarkedUpdated)
        {
            var updatedBookmark = _bookmarkService.GetBookmarkedTitle(username, titleId);

            var result = _mapper.BookmarkTitleDto(updatedBookmark);

            return Ok(result);
        }

        return NotFound("No bookmark found");
    }


    // PERSON BOOKMARKS //

    [HttpPost("persons/{personId}", Name = nameof(CreateBookmarkPerson))]
    [Authorize]

    public IActionResult CreateBookmarkPerson(string personId, [FromBody] UpdateBookmarkDto? dto)
    {
        var username = HttpContext.User.Identity.Name;
        var note = dto?.Note;
       
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
    [Authorize]

    public IActionResult GetBookmarkedPersonsUser([FromQuery]  PageSettings pagesettings)
    {
        var username = HttpContext.User.Identity.Name;

        var bookmarks = _bookmarkService.GetBookmarkPersons(username, pagesettings.Page, pagesettings.PageSize);

        if (bookmarks.TotalNumberOfItems == 0)
        {
            return NotFound("No bookmarks found");
        }

        var bookmarkDto = bookmarks.Items?.Select(bp => _mapper.BookmarkPersonDto(bp)).ToList();

        var result = CreatePaging(nameof(GetBookmarkedPersonsUser), bookmarkDto, bookmarks.TotalNumberOfItems.Value, pagesettings);

        return Ok(result);
    }

    [HttpPut("persons/{personId}")]
    [Authorize]

    public IActionResult UpdateBookmarkPerson(string personId, [FromBody] UpdateBookmarkDto? dto)
    {
        var username = HttpContext.User.Identity.Name;
        var note = dto?.Note;

        var bookmarkUpdated = _bookmarkService.UpdateBookmarkPerson(personId, username, note);

        if (bookmarkUpdated)
        {
            var updatedBookmark = _bookmarkService.GetBookmarkedPerson(username, personId);

            var result = _mapper.BookmarkPersonDto(updatedBookmark);    

            return Ok(result);
        }

        return NotFound("Bookmark not found");
    }

    [HttpDelete("persons/{personId}")]
    [Authorize]

    public IActionResult DeleteBookmarkPerson(string personId)
    {
        var username = HttpContext.User.Identity.Name;

        var deletedBookmark = _bookmarkService.DeleteBookmarkPerson(personId, username);

        if (deletedBookmark == null)
        {
            return NotFound("Bookmark not Found");
        }

        return Ok("Bookmark deleted");
    }

}

