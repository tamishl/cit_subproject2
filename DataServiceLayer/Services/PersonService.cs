using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using DataServiceLayer.Services.Interfaces;

namespace DataServiceLayer.Services;

public class PersonService : IPersonService
{
    private MovieDbContext _dbContext;

    public PersonService()
    {
        _dbContext = new MovieDbContext();
    }


    // Get paginated list of people by name search
    public PagedResultDto<PersonSummaryDto> GetPeopleByName(string search, int page = 0, int pageSize = 10)
    {
        var query = _dbContext.Persons.Where(p => EF.Functions.ILike(p.Name, $"%{search}%"));

        var items = query.OrderBy(p => p.Id)
                         .Skip(page * pageSize)
                         .Take(pageSize)
                         .Select(p => p.Adapt<PersonSummaryDto>())
                         .ToList();

        return new PagedResultDto<PersonSummaryDto>
        {
            Items = items,
            TotalNumberOfItems = query.Count()
        };
    }


    // Get detailed information about a person
    public PersonDetailsDto? GetPerson(string id)
    {

        var person = _dbContext.Persons
        .Include(p => p.KnownFor)  // Include KnownFor titles
        .FirstOrDefault(p => p.Id == id);  

        // If the person is not found, return null
        if (person == null) return null;


        var personDetailsDto = person.Adapt<PersonDetailsDto>();


        personDetailsDto.Professions = person.Professions?.Select(pr => pr.Name).ToList();
        personDetailsDto.KnownForTitles = person.KnownFor?.Select(t => t.PrimaryTitle).ToList();

        return personDetailsDto;
    }

    // Get paginated list of credits for a person
    public PagedResultDto<PersonCreditDto> GetPersonCredits(string id, int page = 0, int pageSize = 10)
    {
        var query = _dbContext.Castings.Where(c => c.PersonId == id)
                                        .Include(c => c.Title);

        var items = query.Skip(page * pageSize)
                            .Take(pageSize)
                            .Select(c => c.Adapt<PersonCreditDto>())
                            .ToList();

        return new PagedResultDto<PersonCreditDto>
        {
            Items = items,
            TotalNumberOfItems = query.Count()
        };
    }

}
