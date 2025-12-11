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


[Route("api/people/{personId}/rating")]
[ApiController]

public class PersonRatingController : BaseController
{
    private IRatingService _ratingService;
    private Mapper _mapper;

    public PersonRatingController(IRatingService ratingService, LinkGenerator linkGenerator, Mapper mapper)
     : base(linkGenerator)
    {
        _ratingService = ratingService;
        _mapper = mapper;
    }

    [HttpGet(Name = nameof(GetPersonRating))]
    public IActionResult GetPersonRating(string personId)
    {
        try
        {
            var personRating = _ratingService.GetPersonRating(personId);

            var dto = _mapper.PersonRatingDto(personRating);

            return Ok(personRating);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}