using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using DataServiceLayer.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Services
{
    public class UserService : IUserService
    {
        private MovieDbContext _dbContext;

        public UserService()
        {
            _dbContext = new MovieDbContext();
        }

        public User CreateUser(string username, string password, string firstName, string lastName, string email, string salt)
        {
            if (UserNameExist(username) || string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"Username '{username}' is already taken or is invalid.");
            }

            if (EmailExist(email) || string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException($"Email '{email}' is already registered or is invalid.");

            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Salt = salt
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            return newUser;

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

        public bool UpdateUser(User updatedUser)
        {
            var existingUser = GetUser(updatedUser.Username);

            if (existingUser == null)
            {
                return false;
            }

            if (_dbContext.Users.Any(u => EF.Functions.ILike(u.Email, updatedUser.Email)
                                  && u.Username != updatedUser.Username))
            {
                return false;
            }

      
            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Password = updatedUser.Password; // need to do something with password

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
    }
}
