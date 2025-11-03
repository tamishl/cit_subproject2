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
    public class RatingService : IRatingService
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
            UpdateTitleRatings(rating);
            UpdatePersonRatings(rating);
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

        public void UpdateTitleRatings(Rating rating)
        {

            var titleRating = _dbContext.TitleRatings.FirstOrDefault(t => t.TitleId == rating.Title.Id);

            if (titleRating == null)
            {
                var firstRating = new TitleRating
                {
                    Title = rating.Title,
                    AverageRating = rating.RatingValue,
                    Votes = 1
                };
                _dbContext.TitleRatings.Add(firstRating);
            }

            else
            {
                titleRating.Votes += 1;
                titleRating.AverageRating = (titleRating.AverageRating * (titleRating.Votes - 1) + rating.RatingValue) / titleRating.Votes;
            }

            //_dbContext.SaveChanges();

        }

        public void UpdatePersonRatings(Rating rating)
        {
            // get all persons in the rated title
            var persons = _dbContext.Castings.Where(c => c.TitleId == rating.Title.Id)
                                               .Select(c => c.PersonId)
                                               .ToList();

            // for each person, update their rating
            foreach (var person in persons)
            {
                // get person rating if it does not exist, create it
                var personRating = _dbContext.PersonRatings.FirstOrDefault(pr => pr.PersonId == person);
                
                if (personRating == null)
                {
                    var newPersonRating = new PersonRating
                    {
                        PersonId = person,
                        AverageRating = rating.RatingValue,
                        Votes = 1
                    };

                    _dbContext.PersonRatings.Add(newPersonRating);
                }

                // else, recalculate the person rating
                else
                {
                    // get all titles for the person
                    var Titles = _dbContext.Castings.Where(c => c.PersonId == person)
                                                        .Select(c => c.TitleId)
                                                        .ToList();

                    // get all title ratings for those titles
                    var TitleRatings = _dbContext.TitleRatings.Where(tr => Titles.Contains(tr.TitleId))
                                                             .ToList();

                    double score = 0;
                    var totalVotes = 0;

                    // for each title, calculate the weighted score
                    foreach (var title in TitleRatings)

                    {
                        var title_score = (double)title.Votes * title.AverageRating;

                        score += title_score;
                        totalVotes += title.Votes;
                    }

                    // update person rating
                    personRating.Votes = totalVotes;
                    personRating.AverageRating = score / totalVotes;
                }
            }
        }
    }
}
