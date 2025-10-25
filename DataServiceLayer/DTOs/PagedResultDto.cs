using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.DTOs;

public class PagedResultDto<T>
{
    public IList<T> Items { get; set; }
    public int? NumberOfItems { get; set; }
}
