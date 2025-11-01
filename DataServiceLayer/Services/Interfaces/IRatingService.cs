using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Services.Interfaces
{
    public interface IRatingService
    {

        // Dataservice Ratings
        public Rating CreateRating(string titleId, string username, int ratingValue);
        public PagedResultDto<RatingDto> GetUserRatings(User user, int page = 0, int pageSize = 10);
        public PagedResultDto<RatingDto> GetAllRatings(int page = 0, int pageSize = 10);
        public bool DeleteRating(string titleId, string username);

    }
}
