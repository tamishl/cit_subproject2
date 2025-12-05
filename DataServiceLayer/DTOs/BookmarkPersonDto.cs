using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.DTOs
{
    public class BookmarkPersonDto
    {
        public string Username { get; set; }
        public string PersonId { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
