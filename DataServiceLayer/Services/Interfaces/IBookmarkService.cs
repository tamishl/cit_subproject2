using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Services.Interfaces
{
    public interface IBookmarkService
    {
        public BookmarkTitle CreateBookmarkTitle(string titleId, string username, string note);
        public BookmarkTitleDto GetBookmarkedTitle(string username, string titleId);
        public PagedResultDto<BookmarkTitleDto> GetBookmarkedTitles(string username, int page = 0, int pageSize = 10);
        public PagedResultDto<BookmarkTitleDto> GetAllTitleBookmarks(int page = 0, int pageSize = 10);
        public BookmarkTitle? DeleteBookmarkTitle(string titleId, string username);
        public bool UpdateBookmarkTitle(string titleId, string username, string note);
        public BookmarkPerson CreateBookmarkPerson(string personId, string username, string note);
        public BookmarkPersonDto GetBookmarkedPerson(string username, string personId);
        public PagedResultDto<BookmarkPersonDto> GetBookmarkPersons(string username, int page = 0, int pageSize = 10);
        public PagedResultDto<BookmarkPersonDto> GetAllPersonBookmarks(int page = 0, int pageSize = 10);
        public BookmarkPerson? DeleteBookmarkPerson(string personId, string username);
        public bool UpdateBookmarkPerson(string personId, string username, string note);
       
    }
}
