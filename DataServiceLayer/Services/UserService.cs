using DataAccesLayer.DTOs;
using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using DataServiceLayer.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Services
{
    public class UserService : IUserService
    {
        private MovieDbContext _dbContext;
        private Hashing _hashing;
        private IConfiguration _configuration;

        public UserService(MovieDbContext dbContext, Hashing hashing, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _hashing = hashing;
            _configuration = configuration;
        }

        public User CreateUser(string username, string password, string? firstName, string? lastName, string email)
        {
            if (UserNameExist(username) || string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"Username is already taken or invalid");
            }

            if (EmailExist(email) || string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException($"Email is already registered or is invalid.");
            }

            if (!passwordIsOK(password, username))
            {
                throw new ArgumentException("Password does not meet the requirements.");
            }

            (var hashPassword, var salt) = _hashing.Hash(password);

            var newUser = new User
            {
                Username = username,
                Password = hashPassword,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Salt = salt
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            return newUser;
        }

        public string Login(string username, string password)
        {
            var user = GetUser(username);

            if (user == null)
            {
                throw new ArgumentException("No user found");
            }

            var verified = _hashing.Verify(password, user.Password, user.Salt);

            if (verified) 
            {
                var token = CreateToken(username);
                return token;
            }
            throw new ArgumentException("Ínvalid password");
        }

        public void ChangePassword(string username, string oldPassword, string newPassword, string checkNewPassword)
        {
            var user = GetUser(username);

            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            if (newPassword != checkNewPassword)
            {
                throw new ArgumentException("Passwordd do not match");
            }

            if (!passwordIsOK(newPassword, username))
            {
                throw new ArgumentException("Password does not meet the requirements.");
            }

            var verified = _hashing.Verify(oldPassword, user.Password, user.Salt);

            if (!verified)
            {
                throw new ArgumentException("Password is incorrect");
            }

            (var hashPassword, var salt) = _hashing.Hash(newPassword);

            user.Password = hashPassword;
            user.Salt = salt;
            _dbContext.SaveChanges();
        }

        public User? GetUser(string username)
        {
            return _dbContext.Users.FirstOrDefault(u => EF.Functions.ILike(u.Username, username));
        }
        public User? GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => EF.Functions.ILike(u.Email, email));
        }

        public PagedResultDto<UserMinimumDetailsDto> GetAllUsers(int page = 0, int pageSize = 10)
        {
            var query = _dbContext.Users
                .Select(u => new UserMinimumDetailsDto
                {
                    Username = u.Username,
                    Email = u.Email
                });

            var items = query.OrderBy(t => t.Username)
                             .Skip(page * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new PagedResultDto<UserMinimumDetailsDto>
            {
                Items = items,
                TotalNumberOfItems = query.Count()
            };
        }
        public bool UpdateUser(string username, UpdateUserDto updatedUser)
        {
            var existingUser = GetUser(username);

            if (existingUser == null)
            {
                throw new ArgumentException("User not found.");
            }

            if (_dbContext.Users.Any(u => EF.Functions.ILike(u.Email, updatedUser.Email)
                                  && u.Username != username))
            {
                throw new ArgumentException("Email is already registered");
            }

      
            existingUser.Email = updatedUser.Email;
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;

            _dbContext.SaveChanges();
            return true;
        }


        public User DeleteUser(User user)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return user;
        }

        public bool DeleteUser(string username)
        {
            var user = GetUser(username);
            if (user == null)
            {
                return false;
            }
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return true;
        }


        // helpermethods

        private bool UserNameExist(string username)
        {
            if (_dbContext.Users.Any(u => EF.Functions.ILike(u.Username, username)))
            {
                return true;
            }
            return false;
        }

        private bool EmailExist(string email)
        {
            if (_dbContext.Users.Any(u => EF.Functions.ILike(u.Email, email)))
            {
                return true;
            }
            return false;
        }


        private bool passwordIsOK(string password, string username)
        {
            if (password.Length < 8)
            {
                throw new ArgumentException("Password must contain more than eight characters");

            }
            if (password.Contains(username, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Password must not contain the username");

            }
            return true;
        }

        private string CreateToken(string username)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            //new Claim(ClaimTypes.Role, user.Role)
        };

            var secret = _configuration.GetSection("Auth:Secret").Value;
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
