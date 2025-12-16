using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.DTOs
{
    public class RatingDto
    {
        public string TitleId { get; set; }
        public string TitleName { get; set; }
        public string? Poster { get; set; }
        public string? Plot { get; set; }
        public int RatingValue { get; set; }
        public double AverageRating { get; set; }
        public int Votes { get; set; }
    }
}
