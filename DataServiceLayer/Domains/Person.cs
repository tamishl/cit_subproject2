using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Domains;

public class Person
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? BirthYear { get; set; }
    public string? DeathYear { get; set; }
    public ICollection<Casting>? Castings { get; set; }

    public ICollection<Profession>? Professions { get; set; }

    public ICollection<Title>? KnownFor { get; set; }
    //public ICollection<Title>? DirectorOf { get; set; }
    //public ICollection<Title>? WriterOf { get; set; }

    public ICollection<BookmarkPerson>? Bookmarks { get; set; }
}
