using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Domains
{
    public class Title
    {
        public string Id { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string? StartYear { get; set; }
        public string? EndYear { get; set; }
        public int RuntimeMinutes { get; set; }
        public string? Plot { get; set; }
        public string? Poster { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<Casting>? Cast { get; set; }
        public TitleType Type { get; set; }
        public ICollection<Genre>? Genres { get; set; }
        public ICollection<Person>? Directors { get; set; }
        public ICollection<Person>? Writers { get; set; }
        public ICollection<Person>? KnownForPersons { get; set; }

        //public ICollection<User>? BookmarkedBy { get; set; }




    }
}
