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


[Route("api/titles/{titleId}/rating")]
[ApiController]

public class TitleRatingController : BaseController
{
    private IRatingService _ratingService;
    private Mapper _mapper;

    public TitleRatingController(IRatingService ratingService, LinkGenerator linkGenerator, Mapper mapper)
     : base(linkGenerator)
    {
        _ratingService = ratingService;
        _mapper = mapper;
    }


    [HttpGet("groupedrating", Name = nameof(GetTitleRatingsByGroup))]

    public IActionResult GetTitleRatingsByGroup(string titleId)
    {
        try
        {
            var titleRatingByGroup = _ratingService.GetTitleRatingByGroup(titleId);

            var dto = _mapper.TitleRatingByGroupDto(titleRatingByGroup, titleId);

            return Ok(dto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet(Name = nameof(GetTitleRating))]
    public IActionResult GetTitleRating(string titleId)
    {
        try
        {
            var titleRating = _ratingService.GetTitleRating(titleId);
            
            var dto = _mapper.TitleRatingDto(titleRating);

            return Ok(titleRating);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

