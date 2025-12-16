using DataAccesLayer.DTOs;
using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using DataServiceLayer.Services;
using DataServiceLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using WebServiceLayer.DTOs;
using WebServiceLayer.Services;


namespace WebServiceLayer.Controllers;


[Route("api/users/me/ratings")]
[ApiController]

public class UserRatingController : BaseController
{
    private IRatingService _ratingService;
    private Mapper _mapper;

    public UserRatingController(IRatingService ratingService, LinkGenerator linkGenerator, Mapper mapper)
     : base(linkGenerator)
    {
        _ratingService = ratingService;
        _mapper = mapper;
    }

    [HttpPost("{titleId}")]
    [Authorize]
    public IActionResult RateTitle(string titleId, [FromBody] RatingValueDto? dto)
    {
        var username = HttpContext.User.Identity.Name;
        var rating = dto.Rating;
 
        try
        {
            var ratingDto = _ratingService.Rate(titleId, username, rating);

            return Ok(_mapper.CreateRatingDto(ratingDto, titleId));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{titleId}")]
    [Authorize]
    public IActionResult GetUserRatingForTitle(string titleId)
    {
        var username = HttpContext.User.Identity.Name;
        var rating = _ratingService.GetUserRatingTitle(username, titleId);

        if (rating == null)
        {
            return NotFound("No rating found");
        }

        return Ok(rating);
    }

    [HttpGet(Name = nameof(GetUserRatings))]
    [Authorize]
    public IActionResult GetUserRatings([FromQuery] PageSettings pageSettings)
    {
        var username = HttpContext.User.Identity.Name;

        var ratings = _ratingService.GetUserRatings(username, pageSettings.Page, pageSettings.PageSize);

        var ratingDto = ratings.Items.Select(r => _mapper.RatingDto(r)).ToList();

        var result = CreatePaging(nameof(GetUserRatings), ratingDto, ratings.TotalNumberOfItems.Value, pageSettings, new { username = username });

        return Ok(result);
    }

    [HttpDelete("{titleId}")]
    [Authorize]
    public IActionResult DeleteRating(string titleId)
    {
        var username = HttpContext.User.Identity.Name;
        var deleted = _ratingService.DeleteRating(titleId, username);
        
        if (!deleted)
        {
            return NotFound("No rating found");
        }
        return Ok("Rating deleted");
    }

}
