using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.ReadDTOs;

public class TitleReadDto
{
    public string PrimaryTitle { get; set; }
    public int Matches { get; set; }
}
