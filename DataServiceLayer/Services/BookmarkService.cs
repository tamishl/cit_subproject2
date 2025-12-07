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

            if (_dbContext.BookmarkTitles.Any(b => b.TitleId == titleId && b.Username == username))
            {
                throw new ArgumentException($"Bookmark for Title ID {titleId} by User {username} already exists.");
            }

            var bookmark = new BookmarkTitle
            {
                TitleId = titleId.Trim(),
                Username = username.Trim(),
                CreatedAt = DateTime.UtcNow,
                Note = note?.Trim()
            };
            _dbContext.BookmarkTitles.Add(bookmark);
            _dbContext.SaveChanges();
            return bookmark;
        }

        public BookmarkTitleDto GetBookmarkedTitle(string username, string titleId)
        {
            var bookmark = _dbContext.BookmarkTitles.FirstOrDefault(bt => bt.Username == username && bt.TitleId == titleId);

            if (bookmark == null)
            {
                throw new ArgumentException("Bookmark does not exist");
            }

            var bookmarkTitleDto = new BookmarkTitleDto
            {
                TitleId = titleId,
                PrimaryTitle = bookmark.Title.PrimaryTitle,
                Username = username,
                Plot = bookmark.Title.Plot,
                Poster = bookmark.Title.Poster,
                CreatedAt = bookmark.CreatedAt,
            };

            return bookmarkTitleDto;
        }

        public PagedResultDto<BookmarkTitleDto> GetBookmarkedTitles(string username, int page = 0, int pageSize = 10)
        {
            var query = _dbContext.BookmarkTitles.Where(bt => bt.Username == username)
                                                 .Select(bt => new BookmarkTitleDto
                                                 {
                                                     PrimaryTitle = bt.Title.PrimaryTitle,
                                                     Username = bt.Username,
                                                     Note = bt.Note,
                                                     TitleId = bt.TitleId,
                                                     Plot = bt.Title.Plot,
                                                     Poster = bt.Title.Poster,
                                                     CreatedAt = bt.CreatedAt
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
                                                     Username = bt.Username,
                                                     Note = bt.Note,
                                                     TitleId = bt.TitleId,
                                                     Plot = bt.Title.Plot,
                                                     Poster = bt.Title.Poster,
                                                     CreatedAt = bt.CreatedAt
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
            var bookmark = _dbContext.BookmarkTitles.FirstOrDefault(bt => bt.TitleId == titleId && bt.Username == username);

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
            var bookmark = _dbContext.BookmarkTitles.FirstOrDefault(bt => bt.TitleId == titleId && bt.Username == username);

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

            if (_dbContext.BookmarkPersons.Any(b => b.PersonId == personId && b.Username == username))
            {
                throw new ArgumentException($"Bookmark for Person ID {personId} by User {username} already exists.");
            }

            var bookmark = new BookmarkPerson
            {
                PersonId = personId,
                Username = username,
                Note = note,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.BookmarkPersons.Add(bookmark);
            _dbContext.SaveChanges();
            return bookmark;
        }


        public BookmarkPersonDto GetBookmarkedPerson(string username, string personId)
        {
            var bookmark = _dbContext.BookmarkPersons.FirstOrDefault(bp => bp.Username == username && bp.PersonId == personId);

            if (bookmark == null)
            {
                throw new ArgumentException("Bookmark does not exist");
            }

            var bookmarkPersonDto = new BookmarkPersonDto
            {
                PersonId = personId,
                Username = username,
                Name = bookmark.Person.Name,
                Note = bookmark.Note,
                CreatedAt = bookmark.CreatedAt,
            };

            return bookmarkPersonDto;

        }

        public PagedResultDto<BookmarkPersonDto> GetBookmarkPersons(string username, int page = 0, int pageSize = 10)
        {
            var query = _dbContext.BookmarkPersons.Where(bp => bp.Username == username)
                                                  .Select(bp => new BookmarkPersonDto
                                                  {
                                                     Username = username,
                                                     PersonId = bp.PersonId,
                                                     Name = bp.Person.Name,
                                                     Note = bp.Note,
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
                Username = bp.Username,
                PersonId = bp.PersonId,
                Name = bp.Person.Name,
                Note = bp.Note,
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
            var bookmark = _dbContext.BookmarkPersons.FirstOrDefault(bp => bp.PersonId == personId && bp.Username == username);

            if (bookmark != null)
            {
                _dbContext.BookmarkPersons.Remove(bookmark);
                _dbContext.SaveChanges();
                return bookmark;
            }
            return null;
        }

        public bool UpdateBookmarkPerson(string personId, string username, string note)
        {
            var bookmark = _dbContext.BookmarkPersons.FirstOrDefault(bp => bp.PersonId == personId && bp.Username == username);

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
