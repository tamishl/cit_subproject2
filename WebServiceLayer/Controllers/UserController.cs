using DataAccesLayer.DTOs;
using DataServiceLayer.Domains;
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


[Route("api/users")]
[ApiController]

public class UserController : BaseController
{

    private IUserService _userService;
    private Mapper _mapper;

    public UserController(IUserService userService, LinkGenerator linkGenerator, Mapper mapper)
        : base(linkGenerator)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public IActionResult CreateUser([FromBody] CreateUser userDto)
    {
        try
        {
            if (userDto.Password != userDto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match");
            }

            var newUser = _userService.CreateUser(userDto.Username, userDto.Password, userDto?.FirstName, userDto?.LastName, userDto.Email);

            var createdUserDto = _mapper.CreateUserDto(newUser);

            return CreatedAtAction(nameof(GetUser), new { username = createdUserDto.Username }, createdUserDto);
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(Exception)
        {
            return BadRequest("User could not be created");
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginUser loginDto)
    {
        try
        {
            var loginToken = _userService.Login(loginDto.Username, loginDto.Password);

            return Ok(new { username = loginDto.Username, token = loginToken });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("me/changepassword")]
    [Authorize]
    public IActionResult ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var username = HttpContext.User.Identity.Name;

        try
        {
            _userService.ChangePassword(username, dto.OldPassword, dto.NewPassword, dto.ConfirmNewPassword);

            return Ok("Password updated successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("me")]
    [Authorize]
    public IActionResult DeleteUser()
    {
        var username = HttpContext.User.Identity.Name;
        var user = _userService.GetUser(username);
        if (user == null)
        {
            return NotFound("User does not exist");
        }
        _userService.DeleteUser(user);

        return Ok("User deleted successfully");
    }

    [HttpGet("me", Name = nameof(GetUser))]
    [Authorize] 
    public IActionResult GetUser()
    {
        var username = HttpContext.User.Identity.Name;

        var user = _userService.GetUser(username);
        if (user == null)
        {
            return NotFound("User does not exist");
        }

        var userDto = _mapper.CreateUserDto(user);

        return Ok(userDto);
    }

    [HttpGet(Name = nameof(GetAllUsers))]

    public IActionResult GetAllUsers([FromQuery] PageSettings pageSettings)
    {
        var users = _userService.GetAllUsers(pageSettings.Page, pageSettings.PageSize);

        if (users.TotalNumberOfItems == 0)
        {
            return NotFound("No users found");
        }

        var usersDto = users.Items?.Select(u => _mapper.AllUsers(u)).ToList();


        var result = CreatePaging(nameof(GetAllUsers), usersDto, users.TotalNumberOfItems.Value, pageSettings);
        
        return Ok(result);
    }

    [HttpPut("me")]
    [Authorize]
    public IActionResult UpdateUser([FromBody] UpdateUserDto? dto)
    {
        var username = HttpContext.User.Identity.Name;

        var user = _userService.GetUser(username);
        try
        {
            var userUpdated = _userService.UpdateUser(username, dto);
            return Ok("User informatipon have been updated");
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}
