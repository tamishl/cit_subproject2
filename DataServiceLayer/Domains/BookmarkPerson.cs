using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Domains
{
    public class BookmarkPerson
    {
        public Person Person { get; set; }
        public string PersonId { get; set; }
        public User User { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Note { get; set; }
    }
}
