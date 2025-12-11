using DataAccesLayer.DTOs;
using DataAccesLayer.ReadDTOs;
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
        public RatingDto Rate(string titleId, string username, int rating);
        public RatingValueDto? GetUserRatingTitle(string username, string titleId);
        public TitleRatingDto? GetTitleRating(string titleId);
        public PersonRatingDto? GetPersonRating(string personId);
        public PagedResultDto<RatingDto> GetUserRatings(string username, int page = 0, int pageSize = 10);
        public List<RatingByGroupDto>? GetTitleRatingByGroup(string titleId);
        public bool DeleteRating(string titleId, string username);

    }
}
