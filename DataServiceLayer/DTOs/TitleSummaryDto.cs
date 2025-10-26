using DataServiceLayer.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.DTOs;

public class TitleSummaryDto
{
    public string PrimaryTitle { get; set; }
    public string? StartYear { get; set; }
    public string? Poster { get; set; }
    public string TypeId { get; set; }
 
}
