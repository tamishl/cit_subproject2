using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.DTOs;

public class PersonGroupedCastDto
{
    public string PersonId { get; set; }

    public ICollection<string>? ProfessionIds { get; set; }

    public ICollection<string>? CharacterNames { get; set; }
}
