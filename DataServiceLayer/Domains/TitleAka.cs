using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Domains;

public class TitleAka
{
    public string TitleId { get; set; }
    public int Ordering { get; set; }
    public int AkaTitle { get; set; }
    public Title Title { get; set; }
    public string? Region { get; set; }
    public string? Language { get; set; }
    public Context? Context { get; set; }
    public string? Description{ get; set; }
    public bool IsOriginalTitle { get; set; }
}
