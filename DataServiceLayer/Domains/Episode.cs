using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Domains;

public class Episode
{
    public string EpisodeId { get; set; }
    public Title Title { get; set; } // should this be in here?
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }
}
