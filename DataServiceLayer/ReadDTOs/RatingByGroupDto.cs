using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.ReadDTOs
{
    public class RatingByGroupDto
    {
        public int RatingValueGroup { get; set; }
        public long NumVotes { get; set; }
        public decimal PercentageVotes { get; set; }

    }
}
