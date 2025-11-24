using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Services.Interfaces
{
    public interface IPersonService
    {

        PagedResultDto<PersonSummaryDto> GetPersonsByName(string search, int page = 0, int pageSize = 10);

        PersonDetailsDto? GetPerson(string id);
        PagedResultDto<PersonCreditDto> GetPersonCredits(string id, int page = 0, int pageSize = 10);
    }
}
