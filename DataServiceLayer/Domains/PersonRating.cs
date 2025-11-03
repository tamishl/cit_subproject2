using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Domains
{
    public class PersonRating
    {
        public Person Person { get; set; }
        public string PersonId { get; set; }
        public double AverageRating { get; set; }
        public int Votes { get; set; }
    }
}
