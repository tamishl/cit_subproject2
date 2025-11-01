using DataServiceLayer.Domains;
using DataServiceLayer.Services.Interfaces;
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

    public UserController(IUserService userService, LinkGenerator linkGenerator, Hashing hashing)
        : base(linkGenerator)
    {
        _userService = userService;
        _hashing = hashing;
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

        var newUser = _userService.CreateUser(username: userDto.Username,
                                password: hashPassword,
                                firstName: userDto.FirstName,
                                lastName: userDto.LastName,
                                email: userDto.Email,
                                salt: salt);


        return Ok(newUser);
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
            new Claim(ClaimTypes.Name, user.Username)

        };

        var secret = "asdjkfhasdjkfhasdjkl234123fhasjkldhfasdjkfhasdjkfhasdjkl234123fhasjkldhf";
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




}
