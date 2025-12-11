using DataAccesLayer.DTOs;
using DataAccesLayer.ReadDTOs;
using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using DataServiceLayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Services
{
    public class RatingService : IRatingService
    {
        private MovieDbContext _dbContext;

        public RatingService()
        {
            _dbContext = new MovieDbContext();
        }

        public RatingDto Rate(string titleId, string username, int rating)
        {
            
            if (_dbContext.Titles.FirstOrDefault(t => t.Id == titleId) == null)
            {
                throw new ArgumentException("Title ID does not exist");
            }

            if (rating < 1 || rating > 10)
            {
                throw new ArgumentException("Rating must be between 1 and 10");
            }

            var sql = "SELECT rate(@p_title, @p_user, @p_rating);";

            _dbContext.Database.ExecuteSqlRaw(sql,
                new NpgsqlParameter("p_title", titleId),
                new NpgsqlParameter("p_user", username),
                new NpgsqlParameter("p_rating", rating));

            var latestRating = _dbContext.Ratings.Include(r => r.Title)
                                                 .Where(r => r.TitleId == titleId && r.Username == username)
                                                 .OrderByDescending(r => r.RatingDate)
                                                 .FirstOrDefault();

            if (latestRating == null)
            {
                throw new InvalidOperationException("Rating was not saved correctly");
            }

            return new RatingDto
            {
                TitleId = latestRating.TitleId,
                TitleName = latestRating.Title.PrimaryTitle,
                Poster = latestRating.Title.Poster,
                Plot = latestRating.Title.Plot,
                RatingValue = latestRating.RatingValue
            };
        }

        public RatingValueDto? GetUserRatingTitle(string username, string titleId)
        {
            var latestRating = _dbContext.Ratings.Include(r => r.Title)
                                                .Where(r => r.TitleId == titleId && r.Username == username)
                                                .OrderByDescending(r => r.RatingDate)
                                                .FirstOrDefault();
            if (latestRating == null) 
            { 
                return null;
            }

            return new RatingValueDto
            {
                Rating = latestRating.RatingValue
            };
        }

        public TitleRatingDto? GetTitleRating(string titleId)
        {
            var titleRating = _dbContext.TitleRatings
                                        .FirstOrDefault(tr => tr.TitleId == titleId);
            if (titleRating != null)
            {
                return new TitleRatingDto
                {
                    TitleId = titleRating.TitleId,
                    AverageRating = titleRating.AverageRating,
                    Votes = titleRating.Votes
                };
            }
            throw new ArgumentException("No Rating yet");
        }

        public PersonRatingDto? GetPersonRating(string personId)
        {
            var personRating = _dbContext.PersonRatings
                                        .FirstOrDefault(pr => pr.PersonId == personId);
            if (personRating != null)
            {
                return new PersonRatingDto
                {
                    PersonId = personRating.PersonId,
                    AverageRating = personRating.AverageRating
                };
            }
            throw new ArgumentException("No Rating yet");
        }


        public PagedResultDto<RatingDto> GetUserRatings(string username, int page = 0, int pageSize = 10)
        {
            var query = _dbContext.Ratings.Include(r => r.Title)
                                          .Where(r => r.Username == username)
                                          .ToList()
                                          .GroupBy(r => r.TitleId)
                                          .Select(g => g.OrderByDescending(r => r.RatingDate).First())
                                          .Select(r => new RatingDto
                                          {
                                              TitleId = r.TitleId,
                                              TitleName = r.Title.PrimaryTitle,
                                              Poster = r.Title.Poster,
                                              Plot = r.Title.Plot,
                                              RatingValue = r.RatingValue
                                          });

            var items = query.Skip(page * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new PagedResultDto<RatingDto>
            {
                Items = items,
                TotalNumberOfItems = query.Count()
            };
        }

        public List<RatingByGroupDto>? GetTitleRatingByGroup(string titleId)
        {
            var title = _dbContext.Titles.FirstOrDefault(t => t.Id == titleId);

            if (title == null)
            {
                throw new ArgumentException("Title does not exist");
            }
            // PostgreSQL function call
            var result = _dbContext.RatingByGroupDtos.FromSqlInterpolated($"SELECT * FROM get_rating_by_group({titleId})")
                                    .ToList();

            return result;
        }



        public bool DeleteRating(string titleId, string username)
        {
            var rating = _dbContext.Ratings.FirstOrDefault(r => r.Username == username && r.TitleId == titleId);

            if (rating == null)
            {
                return false;
            }

            var sql = "SELECT delete_rating(@p_title, @p_user);";

            _dbContext.Database.ExecuteSqlRaw(sql,
                new NpgsqlParameter("p_title", titleId),
                new NpgsqlParameter("p_user", username));

            return true;
        }

    }
}
