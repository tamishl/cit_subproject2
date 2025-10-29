using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mapster;

namespace DataServiceLayer.Services;

public class TitleService: ITitleService
{
    private MovieDbContext _dbContext;

    public TitleService()
    {
        _dbContext = new MovieDbContext();
    }



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
  


    public PagedResultDto<TitleSummaryDto> GetTitlesByType(string type, int page = 0, int pageSize = 10)
    {
        var query = _dbContext.TitleTypes.FirstOrDefault(t => t.Id == type).Titles;
        var items = query.OrderBy(t => t.Id)
                         .Skip(page * pageSize)
                         .Take(pageSize)
                         .Select(t => t.Adapt<TitleSummaryDto>())
                         .ToList();

        return new PagedResultDto<TitleSummaryDto>
        {
            Items = items,
            TotalNumberOfItems = items?.Count
        };
    }

    public TitleDto? GetTitle(string id)
    {
        return _dbContext.Titles.Include(t => t.Ratings)
                                .Include(t => t.Cast)
                                .Include(t => t.Genres)
                                .Include(t => t.Type)
                                .Include(t => t.Directors)
                                .Include(t => t.Writers)
                                .FirstOrDefault(t => t.Id == id).Adapt<TitleDto>();
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


    public int GetTitleCount()
    {
            return _dbContext.Titles.Count();
    }
}
