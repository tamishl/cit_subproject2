using DataServiceLayer.Domains;
using DataServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers;


[Route("api/titles")]
[ApiController]

public class TitleController: BaseController
{
    
    private ITitleService _titleService;


    public TitleController(ITitleService titleService, LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _titleService = titleService;
    }


    [HttpGet (Name = nameof(GetTitlesByName))]
    public IActionResult GetTitlesByName([FromQuery] PageSettings pageSettings, string? search = null)
    {
        // without serach parameter, return all titles paged
        if (string.IsNullOrEmpty(search))
        {
            var pagedResult = _titleService.GetTitles(pageSettings.Page, pageSettings.PageSize);

            var result = CreatePaging(nameof(GetTitlesByName), pagedResult.Items, pagedResult.NumberOfItems.Value, pageSettings);

            return Ok(result);

        }

        // with search parameter, return filtered titles paged
        else
        {
            var pagedResult = _titleService.GetTitlesByName(search, pageSettings.Page, pageSettings.PageSize);

            var result = CreatePaging(nameof(GetTitlesByName), pagedResult.Items, pagedResult.NumberOfItems.Value, pageSettings);

            return Ok(result);
        }
        //return Ok(_titleService.GetTitlesByName(pageSettings.Page, pageSettings.PageSize, search));
    }


    //[HttpGet("{movies}")]
    //public IActionResult GetMovies([)
    //{
    //    var titles = _titleService.GetMovies();

    //    return Ok(movies);
    //}

}
