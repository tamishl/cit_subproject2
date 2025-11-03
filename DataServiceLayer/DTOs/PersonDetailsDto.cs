using DataServiceLayer.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.DTOs;

public class PersonDetailsDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? BirthYear { get; set; }
    public string? DeathYear { get; set; }
    public List<string>? Professions { get; set; }
    public List<string>? KnownForTitles { get; set; }

}
