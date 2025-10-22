using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Domains
{
    public class TitleType
    {
        public string Id { get; set; }
        public ICollection<Title>? Titles { get; set; }
    }
}
