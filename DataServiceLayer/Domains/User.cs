using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataServiceLayer.Domains
{
    public class User
    {
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Search>? SearchHistory { get; set; }
        public ICollection<Rating>? RatedTitles { get; set; }
        public ICollection<Title>? BookmarkedTitles { get; set; }
        public ICollection<Person>? BookmarkedPersons { get; set; }
    }
}
