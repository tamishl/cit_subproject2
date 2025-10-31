using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.DTOs
{
    public class BookmarkTitleDto
    {
        public string Username { get; set; }
        public string PrimaryTitle { get; set; }
        public string Plot { get; set; }
        public string? Poster { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
