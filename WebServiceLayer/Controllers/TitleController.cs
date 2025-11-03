using DataServiceLayer.Domains;
using DataServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.DTOs;

namespace WebServiceLayer.Controllers;


[Route("api/titles")]
[ApiController]

public class TitleController : BaseController
{

    private ITitleService _titleService;


    public TitleController(ITitleService titleService, LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _titleService = titleService;
    }


    [HttpGet(Name = nameof(GetTitlesByName))]
    public IActionResult GetTitlesByName([FromQuery] PageSettings pageSettings, string? search = null)
    {
        // without serach value, return all titles paged
        if (string.IsNullOrEmpty(search))
        {
            var pagedResult = _titleService.GetTitles(pageSettings.Page, pageSettings.PageSize);

            var result = CreatePaging(nameof(GetTitlesByName), pagedResult.Items, pagedResult.TotalNumberOfItems.Value, pageSettings);

            return Ok(result);

        }

        // with search value, return filtered titles paged
        else
        {
            var pagedResult = _titleService.GetTitlesByName(search, pageSettings.Page, pageSettings.PageSize);

            var result = CreatePaging(nameof(GetTitlesByName), pagedResult.Items, pagedResult.TotalNumberOfItems.Value, pageSettings);

            return Ok(result);
        }
        //return Ok(_titleService.GetTitlesByName(pageSettings.Page, pageSettings.PageSize, search));
    }

    [HttpPost(Name = nameof(GetTitlesBySearch))]
    public IActionResult GetTitlesBySearch([FromQuery] PageSettings pageSettings, [FromBody] SearchDto? searchDto = null)
    {
        if (searchDto is null)
        {
            var pagedResult = _titleService.GetTitles(pageSettings.Page, pageSettings.PageSize);

            var result = CreatePaging(nameof(GetTitlesBySearch), pagedResult.Items, pagedResult.TotalNumberOfItems.Value, pageSettings);

            return Ok(result);
        }

        else
        {
            var result = _titleService.GetTitlesBySearch(searchDto.Search);

            return Ok(result);
        }
    }


    [HttpGet("{id}")]
    public IActionResult GetTitle(string id)
    {
        var result = _titleService.GetTitle(id);

        return result is not null ? Ok(result) : NotFound("Title does not exist");
    }


    //[HttpGet("{titleType}")]
    //public IActionResult GetMovies(string titleType)
    //{
    //    var titles = _titleService.GetMovies();

    //    return Ok(movies);
    //}

}
