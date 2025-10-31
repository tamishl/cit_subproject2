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
        public PagedResultDto<BookmarkTitleDto> GetBookmarkedTitles(string username);
        public PagedResultDto<BookmarkTitleDto> GetAllTitleBookmarks();
        BookmarkTitle? DeleteBookmarkTitle(string titleId, string username);
        public bool UpdateBookmarkTitle(string titleId, string username, string note);
        public BookmarkPerson CreateBookmarkPerson(string personId, string username, string note);
        public PagedResultDto<BookmarkPersonDto> GetAllPersonBookmarks();
        public PagedResultDto<BookmarkPersonDto> GetBookmarkPersons(string username);
        public void DeleteBookmarkPerson(string personId, string username);
        public void UpdateBookmarkPerson(string personId, string username, string note);
       
    }
}
