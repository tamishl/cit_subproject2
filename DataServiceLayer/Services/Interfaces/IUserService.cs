using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Services.Interfaces
{
    public interface IUserService
    {
        public User CreateUser(string username, string firstName, string lastName, string email, string password, string salt);
        public User? GetUser(string username);
        public User? GetUserByEmail(string email);
        public PagedResultDto<UserMinimumDetailsDto> GetAllUsers(int page = 0, int pageSize = 10);
        public bool UpdateUser(User user);
        public User DeleteUser(User user);
        public bool DeleteUser(string username);
    }
}
