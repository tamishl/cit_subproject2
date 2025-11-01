using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using DataServiceLayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Services
{
    public class RatingService :IRatingService
    {
        private MovieDbContext _dbContext;

        public RatingService()
        {
            _dbContext = new MovieDbContext();
        }

        public Rating CreateRating(string titleId, string username, int ratingValue)
        {
            if (!_dbContext.Titles.Any(t => t.Id == titleId))
            {
                throw new ArgumentException($"Title does not exist");
            }

            if (!_dbContext.Users.Any(t => t.Username == username))
            {
                throw new ArgumentException($"Missing a valid.");
            }

            if (ratingValue < 1 || ratingValue > 10)
            {
                throw new ArgumentException($"Rating value must be between 1 and 10.");
            }

            var existingRating = _dbContext.Ratings.FirstOrDefault(r => r.Title.Id == titleId && r.User.Username == username);

            if (existingRating != null)
            {
                //if rating exists, update it. I dont know what is the best way of doing it. 
                existingRating.RatingValue = ratingValue;
                existingRating.RatingDate = DateTime.UtcNow;
                _dbContext.SaveChanges();
                return existingRating;
            }

            // if rating does not exist, create it
            var rating = new Rating
            {
                Title = _dbContext.Titles.First(t => t.Id == titleId),
                User = _dbContext.Users.First(u => u.Username == username),
                RatingValue = ratingValue,
                RatingDate = DateTime.UtcNow
            };
            _dbContext.Ratings.Add(rating);
            _dbContext.SaveChanges();
            return rating;
        }

        public PagedResultDto<RatingDto> GetUserRatings(User user, int page = 0, int pageSize = 10)
        {
            var query = _dbContext.Ratings.Where(r => r.User == user)
                                          .GroupBy(r => r.Title.Id)
                                          .Select(g => g.OrderByDescending(r => r.RatingDate).First())
                                          .Select(r => new RatingDto
                                          {
                                              TitleName = r.Title.PrimaryTitle,
                                              Poster = r.Title.Poster,
                                              Plot = r.Title.Plot,
                                              Rating = r.RatingValue
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

        public PagedResultDto<RatingDto> GetAllRatings(int page = 0, int pageSize = 10)
        {
            var query = _dbContext.Ratings.GroupBy(r => new { r.Title.Id, r.User.Username })
                                          .Select(g => g.OrderByDescending(r => r.RatingDate).First())
                                          .Select(r => new RatingDto
                                          {
                                              TitleName = r.Title.PrimaryTitle,
                                              Poster = r.Title.Poster,
                                              Plot = r.Title.Plot,
                                              Rating = r.RatingValue
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

        public bool DeleteRating(string titleId, string username)
        {
            var rating = _dbContext.Ratings.FirstOrDefault(r => r.User.Username == username && r.Title.Id == titleId);

            if (rating == null)
            {
                return false;
            }

            _dbContext.Ratings.Remove(rating);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
