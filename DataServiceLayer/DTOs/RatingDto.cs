using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.DTOs
{
    public class RatingDto
    {
        public string TitleName { get; set; }
        public string? Poster { get; set; }
        public string? Plot { get; set; }
        public int Rating { get; set; }
    }
}
