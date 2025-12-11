using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.ReadDTOs
{
    public class GetTitleRatingByGroupDto
    {
        public string TitleId { get; set; }
        public string Url { get; set; }
        public List<RatingByGroupDto> RatingGroups { get; set; }
    }
}
