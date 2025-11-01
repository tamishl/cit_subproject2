using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Domains
{
    public  class TitleRating
    {
        public Title Title { get; set; }
        public string TitleId { get; set; }
        public float AverageRating { get; set; }
        public int Votes { get; set; }
    }
}
