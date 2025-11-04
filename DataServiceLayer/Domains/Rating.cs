using Npgsql.TypeMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Domains;

public class Rating
{
    public int Id { get; set; }
    public int RatingValue { get; set; }
    public DateTime RatingDate { get; set; }
    public Title Title { get; set; }
    public string TitleId { get; set; }
    public User User { get; set; }
    public string Username { get; set; }
}
