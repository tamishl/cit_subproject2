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


[Route("api/User")]
[ApiController]

public class UserController : BaseController
{

    private IUserService _userService;
    private Hashing _hashing;
    private Mapper _mapper;
    private IConfiguration _configuration;

    public UserController(IUserService userService, LinkGenerator linkGenerator, Hashing hashing, Mapper mapper, IConfiguration configuration)
        : base(linkGenerator)
    {
        _userService = userService;
        _hashing = hashing;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUser userDto)
    {
        Console.WriteLine($"Username: {userDto.Username}, Password: {userDto.Password}");

        var user = _userService.GetUser(userDto.Username);

        if (user != null)
        {
            return BadRequest("Username already exists");
        }

        if (string.IsNullOrWhiteSpace(userDto.Password))
        {
            return BadRequest("Password cannot be empty");
        }

        (var hashPassword, var salt) = _hashing.Hash(userDto.Password);

        Console.WriteLine($"Username: {userDto.Username}, Password: {userDto.Password}, Hashpassword: {hashPassword}, firstname: {userDto.FirstName}");

        var newUser = _userService.CreateUser(userDto.Username, hashPassword, userDto.FirstName, userDto.LastName, userDto.Email, salt);

        var createdUserDto = _mapper.CreateUserDto(newUser);

        return CreatedAtAction(nameof(GetUser), new { username = createdUserDto.Username }, createdUserDto);
    }

    [HttpPut]
    public IActionResult Login([FromBody] LoginUser loginDto)
    {
        Console.WriteLine($"Username: {loginDto.Username}, Password: {loginDto.Password}");

        var user = _userService.GetUser(loginDto.Username);

        if (user == null)
        {
            return BadRequest("No user with that name");
        }

        if (!_hashing.Verify(loginDto.Password, user.Password, user.Salt))
        {
            return BadRequest();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            //new Claim(ClaimTypes.Role, user.Role)
        };

        var secret = _configuration.GetSection("Auth:Secret").Value;
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(20),
            signingCredentials: creds
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { username = user.Username, token = jwt });
    }


    [HttpDelete("{username}")]
    [Authorize]
    public IActionResult DeleteUser(string username)
    {
        var user = _userService.GetUser(username);
        if (user == null)
        {
            return NotFound("User does not exist");
        }
        _userService.DeleteUser(user);

        return Ok("User deleted successfully");
    }

    [HttpGet("{username}", Name = nameof(GetUser))]
    public IActionResult GetUser(string username)
    {
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

    [HttpPut("{username}")]

    public IActionResult UpdateUser(string username)
    {
        var user = _userService.GetUser(username);

        if(user == null)
        {
            return NotFound("No user found");
        }

        var updatedUser = _userService.UpdateUser(user);

        return Ok(updatedUser);
    }






}
