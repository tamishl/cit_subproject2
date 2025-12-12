using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.DTOs
{
    public class PersonRatingDto
    {
        public string PersonId { get; set; }
        public double AverageRating { get; set; }
    }
}
