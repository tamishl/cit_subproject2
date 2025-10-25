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


    [HttpGet]
    public IActionResult GetTitlesByName([FromQuery] PageSettings pageSettings, string? search = null)
    {
        
        if (string.IsNullOrEmpty(search))
        {
            return Ok(_titleService.GetTitles(pageSettings.PageSize, pageSettings.Page));
        }

        return Ok(_titleService.GetTitlesByName(search, false));
    }


    //[HttpGet("{movies}")]
    //public IActionResult GetMovies([)
    //{
    //    var titles = _titleService.GetMovies();

    //    return Ok(movies);
    //}

}
