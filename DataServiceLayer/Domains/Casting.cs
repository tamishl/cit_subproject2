using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Domains;

public class Casting
{
    public Title Title { get; set; }
    public string TitleId { get; set; }
    public Person Person { get; set; }
    public string PersonId { get; set; }
    public Profession Profession{ get; set; }
    public string? Job { get; set; }
    public string? CharacterName { get; set; }
    public int Ordering { get; set; }
    // public string Department { get; set; } 
    // public string Role { get; set; }

}
