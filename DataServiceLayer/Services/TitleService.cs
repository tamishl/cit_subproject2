using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using Mapster;
using DataServiceLayer.Services.Interfaces;

namespace DataServiceLayer.Services;

public class TitleService: ITitleService
{
    private MovieDbContext _dbContext;

    public TitleService()
    {
        _dbContext = new MovieDbContext();
    }


 // improve getting count by using window functions >> get total count in the same query as the query that gets result



    public PagedResultDto<TitleSummaryDto> GetTitles(int page = 0, int pageSize = 10)
    {
        var query = _dbContext.Titles;
        var items = query.OrderBy(t => t.Id)
                         .Skip(page * pageSize)
                         .Take(pageSize)
                         .Select(t => t.Adapt<TitleSummaryDto>())
                         .ToList();

        return new PagedResultDto<TitleSummaryDto>
        {
            Items = items,
            TotalNumberOfItems = _dbContext.Titles.Count()
        };
    }


    public PagedResultDto<TitleSummaryDto> GetTitlesByName(string search, int page = 0, int pageSize = 10)
    {


        //var query = _dbContext.Titles.Where(t => t.PrimaryTitle.Contains(search)); // case sensitive
        var query = _dbContext.Titles.Where(t => EF.Functions.ILike(t.PrimaryTitle, $"%{search}%")); // case insensitive

        var items = query.OrderBy(t => t.Id)
                         .Skip(page * pageSize)
                         .Take(pageSize)
                         .Select(t => t.Adapt<TitleSummaryDto>())
                         .ToList();

        return new PagedResultDto<TitleSummaryDto>
        {
            Items = items,
            TotalNumberOfItems = query.Count()
        };
    }



    public PagedResultDto<TitleSummaryDto>? GetTitlesByGenre(string genreId, int page = 0, int pageSize = 10)
    {
        var query = _dbContext.Titles.Where(t => t.Genres.Any(g => EF.Functions.ILike(g.Id, $"{genreId}")));

        var items = query.OrderBy(t => t.Id)
                         .Skip(page * pageSize)
                         .Take(pageSize)
                         .Select(t => t.Adapt<TitleSummaryDto>())
                         .ToList();

        return new PagedResultDto<TitleSummaryDto>
        {
            Items = items,
            TotalNumberOfItems = query.Count()
        };
    }



    public PagedResultDto<TitleSummaryDto> GetTitlesByType(string typeId, int page = 0, int pageSize = 10)
    {
        var query = _dbContext.Titles.Where(t => EF.Functions.ILike(t.Type.Id, $"{typeId}"));
        var items = query.OrderBy(t => t.Id)
                         .Skip(page * pageSize)
                         .Take(pageSize)
                         .Select(t => t.Adapt<TitleSummaryDto>())
                         .ToList();

        return new PagedResultDto<TitleSummaryDto>
        {
            Items = items,
            TotalNumberOfItems = query.Count()
        };
    }

    public TitleDto? GetTitle(string id)
    {
        return _dbContext.Titles.Select(t => new
        {
            t.Id,
            t.PrimaryTitle,
            t.OriginalTitle,
            t.IsAdult,
            t.StartYear,
            t.EndYear,
            t.RuntimeMinutes,
            t.Plot,
            t.Poster,
            t.TypeId,
            Genres = t.Genres == null ? null : t.Genres.Select(g => g.Id),
            Writers = t.Writers == null ? null: t.Writers.Select(w => w.Name),
            Directors = t.Directors == null ? null : t.Directors.Select(d => d.Name),
            Cast = t.Cast == null ? null : t.Cast.Select(c => new PersonCastDto
            {
                Name = c.Person.Name,
                ProfessionId = c.Profession.Id
            }),
            TitleRating = t.TitleRating == null ? null : new TitleRatingDto
            {
                TitleId = t.TitleRating.TitleId,
                AverageRating = t.TitleRating.AverageRating,
                Votes = t.TitleRating.Votes
            }
                                }).AsSplitQuery()
                                  .FirstOrDefault(t => t.Id == id).Adapt<TitleDto>();
    }



    public TitleSummaryDto? GetTitleSummary(string id)
    {
        return _dbContext.Titles.FirstOrDefault(t => t.Id == id).Adapt<TitleSummaryDto>();
    }

    public PagedResultDto<TitleSummaryDto>? GetAkas(string id)
    {
        //return _dbContext.Titles.FirstOrDefault(t => t.Id == id).Akas
        //                        .Select(ta => new TitleAkaSummaryDto
        //                        {
        //                            Title = ta.TitleName;
        //                        };

        return null;
    }


    public PagedResultDto<TitleSummaryDto> GetTitlesBySearch(string search, int page = 0, int pageSize = 10)
    {
        var query = _dbContext.TitleReadDtos.FromSqlInterpolated($"SELECT * FROM best_match_variadic(VARIADIC {search.Split(' ')})");
        var items = query.Skip(page * pageSize)
                         .Take(pageSize)
                         .Select(t => t.Adapt<TitleSummaryDto>()).ToList();

        return new PagedResultDto<TitleSummaryDto>
        {
            Items = items,
            TotalNumberOfItems = query.Count()
        };
    }


    public int GetTitleCount()
    {
            return _dbContext.Titles.Count();
    }
}
