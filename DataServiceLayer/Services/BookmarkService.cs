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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataServiceLayer.Services
{
    public class BookmarkService : IBookmarkService
    {
        private MovieDbContext _dbContext;

        public BookmarkService()
        {
            _dbContext = new MovieDbContext();
        }

        // Title bookmarks
        public BookmarkTitle CreateBookmarkTitle(string titleId, string username, string note)
        {
            if (!_dbContext.Titles.Any(t => t.Id == titleId))
            {
                throw new ArgumentException($"Title with ID {titleId} does not exist.");
            }

            if (!_dbContext.Users.Any(u => u.Username == username))
            {
                throw new ArgumentException($"User with username {username} does not exist.");
            }

            if (_dbContext.BookmarkTitles.Any(b => b.Title.Id == titleId && b.User.Username == username))
            {
                throw new ArgumentException($"Bookmark for Title ID {titleId} by User {username} already exists.");
            }

            var bookmark = new BookmarkTitle
            {
                Title = _dbContext.Titles.First(t => t.Id == titleId),
                User = _dbContext.Users.First(u => u.Username == username),
                CreatedAt = DateTime.UtcNow,
                Note = note
            };
            _dbContext.BookmarkTitles.Add(bookmark);
            _dbContext.SaveChanges();
            return bookmark;
        }
        public PagedResultDto<BookmarkTitleDto> GetBookmarkedTitles(string username, int page = 0, int pageSize = 10)
        {
            var query = _dbContext.BookmarkTitles.Where(bt => bt.User.Username == username)
                                                 .Select(bt => new BookmarkTitleDto
                                                 {
                                                     PrimaryTitle = bt.Title.PrimaryTitle,
                                                     Username = bt.User.Username,
                                                     Plot = bt.Title.Plot,
                                                     Poster = bt.Title.Poster,
                                                 });
            var items = query.OrderByDescending(bt => bt.CreatedAt)
                             .Skip(page * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new PagedResultDto<BookmarkTitleDto>
            {
                Items = items,
                TotalNumberOfItems = query.Count()
            };
        }

        public PagedResultDto<BookmarkTitleDto> GetAllTitleBookmarks(int page = 0, int pageSize = 10)
        {
            var query = _dbContext.BookmarkTitles.Select(bt => new BookmarkTitleDto
                                                 {
                                                     PrimaryTitle = bt.Title.PrimaryTitle,
                                                     Username = bt.User.Username,
                                                     Plot = bt.Title.Plot,
                                                     Poster = bt.Title.Poster,
                                                 });
            var items = query.OrderByDescending(bt => bt.CreatedAt)
                             .Skip(page * pageSize)
                             .Take(pageSize)
                             .ToList(); 

            return new PagedResultDto<BookmarkTitleDto>
            {
                Items = items,
                TotalNumberOfItems = query.Count()
            };
        }

        public BookmarkTitle? DeleteBookmarkTitle(string titleId, string username)
        {
            var bookmark = _dbContext.BookmarkTitles.FirstOrDefault(bt => bt.Title.Id == titleId && bt.User.Username == username);

            if (bookmark != null)
            {
                _dbContext.BookmarkTitles.Remove(bookmark);
                _dbContext.SaveChanges();
                return bookmark;
            }
            return bookmark;
        }

        public bool UpdateBookmarkTitle(string titleId, string username, string note) // could change to BookmarkTitle as parameter
        {
            var bookmark = _dbContext.BookmarkTitles.FirstOrDefault(bt => bt.Title.Id == titleId && bt.User.Username == username);

            if (bookmark != null)
            {
                bookmark.CreatedAt = DateTime.UtcNow;
                bookmark.Note = note;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }



        // Person bookmarks
        public BookmarkPerson CreateBookmarkPerson(string personId, string username, string note = null)
        {
            if (!_dbContext.Persons.Any(p => p.Id == personId))
            {
                throw new ArgumentException($"Person with ID {personId} does not exist.");
            }

            if (!_dbContext.Users.Any(u => u.Username == username))
            {
                throw new ArgumentException($"User with username {username} does not exist.");
            }

            if (_dbContext.BookmarkPersons.Any(b => b.Person.Id == personId && b.User.Username == username))
            {
                throw new ArgumentException($"Bookmark for Person ID {personId} by User {username} already exists.");
            }

            var bookmark = new BookmarkPerson
            {
                Person = _dbContext.Persons.First(p => p.Id == personId),
                User = _dbContext.Users.First(u => u.Username == username),
                Note = note,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.BookmarkPersons.Add(bookmark);
            _dbContext.SaveChanges();
            return bookmark;
        }

        public PagedResultDto<BookmarkPersonDto> GetBookmarkPersons(string username, int page = 0, int pageSize = 10)
        {
            var query = _dbContext.BookmarkPersons.Where(bp => bp.User.Username == username)
                                                  .Select(bp => new BookmarkPersonDto
                                                  {
                                                     Username = bp.User.Username,
                                                     Name = bp.Person.Name,
                                                     CreatedAt = bp.CreatedAt
                                                  });
            var items = query.OrderByDescending(bt => bt.CreatedAt)
                             .Skip(page * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new PagedResultDto<BookmarkPersonDto>
            {
                Items = items,
                TotalNumberOfItems = query.Count()
            };
        }

        public PagedResultDto<BookmarkPersonDto> GetAllPersonBookmarks(int page = 0, int pageSize = 10)
        {
            var query = _dbContext.BookmarkPersons.Select(bp => new BookmarkPersonDto
            {
                Username = bp.User.Username,
                Name = bp.Person.Name,
                CreatedAt = bp.CreatedAt
            });
            var items = query.OrderByDescending(bt => bt.CreatedAt)
                             .Skip(page * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new PagedResultDto<BookmarkPersonDto>
            {
                Items = items,
                TotalNumberOfItems = query.Count()
            };
        }

        public BookmarkPerson? DeleteBookmarkPerson(string personId, string username)
        {
            var bookmark = _dbContext.BookmarkPersons.FirstOrDefault(bp => bp.Person.Id == personId && bp.User.Username == username);

            if (bookmark != null)
            {
                _dbContext.BookmarkPersons.Remove(bookmark);
                _dbContext.SaveChanges();
                return bookmark;
            }
            return bookmark;
        }

        public bool UpdateBookmarkPerson(string personId, string username, string note)
        {
            var bookmark = _dbContext.BookmarkPersons.FirstOrDefault(bp => bp.Person.Id == personId && bp.User.Username == username);

            if (bookmark != null)
            {
                bookmark.CreatedAt = DateTime.UtcNow;
                bookmark.Note = note;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
