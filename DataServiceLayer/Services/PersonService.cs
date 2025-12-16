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


    // Get paginated list of persons by name search
    public PagedResultDto<PersonSummaryDto> GetPersonsByName(string search, int page = 0, int pageSize = 10)
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

        return _dbContext.Persons.Select(p => new
                                    {
                                        p.Id,
                                        p.Name,
                                        p.BirthYear,
                                        p.DeathYear,
                                        Professions = p.Professions == null ? null : p.Professions.Select(pr => pr.Id),
                                        KnownForTitles = p.KnownForTitles == null ? null :
                                                         p.KnownForTitles.Select(t => new
                                                                                {
                                                                                    t.Id,
                                                                                    t.PrimaryTitle,
                                                                                    t.StartYear,
                                                                                    t.EndYear,
                                                                                    t.Poster,
                                                                                    t.TitleRating
                                                                                })
                                    })
                                 .FirstOrDefault(p => p.Id == id)
                                 .Adapt<PersonDetailsDto>();

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
