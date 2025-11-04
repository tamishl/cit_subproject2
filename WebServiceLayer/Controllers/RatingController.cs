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

    
[Route("api/users/{username}/ratings")]
[ApiController]

public class RatingController : BaseController
{
    private IRatingService _ratingService;
    private Hashing _hashing;
    private Mapper _mapper;
    private IConfiguration _configuration;

    public RatingController(IRatingService ratingService, LinkGenerator linkGenerator, Hashing hashing, Mapper mapper, IConfiguration configuration)
     : base(linkGenerator)
    {
        _ratingService = ratingService;
        _hashing = hashing;
        _mapper = mapper;
        _configuration = configuration;
    }


    [HttpGet(Name = nameof(GetRatings))]
    public IActionResult GetRatings(string username, [FromQuery] PageSettings pageSettings)
    {
        var ratings = _ratingService.GetUserRatings(username, pageSettings.Page, pageSettings.PageSize);


        var ratingDto = ratings.Items.Select(r => _mapper.RatingDto(r)).ToList();

        var result = CreatePaging(nameof(GetRatings), ratingDto, ratings.TotalNumberOfItems.Value, pageSettings);

        return Ok(result);
    }

    
    /*
    [HttpPost]

    public IActionResult CreateRating(string username, Rating rating)
    {
        var createdRating = _ratingService.CreateRating(rating.TitleId, username, rating.RatingValue);

        if (createdRating == null)
        {
            return BadRequest("Could not create rating");
        }

      

        return CreatedAtAction(nameof(GetRatings), new { username = username }, ratingDto);

    }
    */

}
