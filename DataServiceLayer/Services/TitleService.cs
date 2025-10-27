using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Services;

    public class TitleService: ITitleService
    {
    private MovieDbContext _dbContext;

    public TitleService()
    {
        _dbContext = new MovieDbContext();
    }



    public PagedResultDto<TitleSummaryDto> GetTitles(int page = 0, int pageSize = 10, bool includeCount = true)
    {
        var query = _dbContext.Titles;
        var items = query.OrderBy(t => t.Id)
                         .Skip(page * pageSize)
                         .Take(pageSize)
                         .Select(t => new TitleSummaryDto { PrimaryTitle = t.PrimaryTitle,
                                                            StartYear = t.StartYear,
                                                            Poster = t.Poster,
                                                            TypeId = t.Type.Id })
                         .ToList(); 
        if (includeCount)
        { 
            return new PagedResultDto<TitleSummaryDto>
            {
                Items = items,
                TotalNumberOfItems = _dbContext.Titles.Count()
            };
        }

        else
        {
            return new PagedResultDto<TitleSummaryDto>
            {
                Items = items,
                TotalNumberOfItems = null
            };
    } 
    }


    // NOTE: is case sensitive
    public PagedResultDto<TitleSummaryDto> GetTitlesByName(string search, int page = 0, int pageSize = 10, bool includeCount = true)
    {


        //var query = _dbContext.Titles.Where(t => t.PrimaryTitle.Contains(search)); // case sensitive
        var query = _dbContext.Titles.Where(t => EF.Functions.ILike(t.PrimaryTitle, $"%{search}%")); // case insensitive

        var items = query.OrderBy(t => t.Id)
                         .Skip(page * pageSize)
                         .Take(pageSize)
                         .Select(t => new TitleSummaryDto { PrimaryTitle = t.PrimaryTitle,
                                                            StartYear = t.StartYear,
                                                            Poster = t.Poster,
                                                            TypeId = t.Type.Id })
                         .ToList();

        if (includeCount)
            {
                return new PagedResultDto<TitleSummaryDto>
                {
                    Items = items,
                    TotalNumberOfItems = query.Count()
                };
            }
            else
            {
                return new PagedResultDto<TitleSummaryDto>
                {
                    Items = items,
                    TotalNumberOfItems = null
                };
        }
    }


    public int GetTitleCount()
    {
            return _dbContext.Titles.Count();
    }


    //methods below are just for eplxoration
    public string? GetTitleNameById(string id) // only for testing
    {
        return _dbContext.Titles.FirstOrDefault(t => t.Id == id)?.PrimaryTitle;
    }

}
