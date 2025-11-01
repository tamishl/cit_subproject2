using DataServiceLayer.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.DTOs;

public class TitleDto
{
    public string PrimaryTitle { get; set; }
    public string OriginalTitle { get; set; }
    public bool IsAdult { get; set; }
    public string? StartYear { get; set; }
    public string? EndYear { get; set; }
    public int? RuntimeMinutes { get; set; }
    public string? Plot { get; set; }
    public string? Poster { get; set; }
    public ICollection<Rating>? Ratings { get; set; }
    public ICollection<string>? Cast { get; set; }
    public string TypeId { get; set; }
    public ICollection<string>? Genres { get; set; }
    public ICollection<string>? Directors { get; set; }
    public ICollection<string>? Writers { get; set; }
}
