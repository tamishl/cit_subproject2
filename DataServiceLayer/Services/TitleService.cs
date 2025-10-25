using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
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



    public IList<TitleSummaryDto> GetTitles(int max, int page, int pageSize)
    {
        return _dbContext.Titles.Take(max)
                                .Select(t => new TitleSummaryDto { PrimaryTitle = t.PrimaryTitle, StartYear = t.StartYear, Poster = t.Poster, Type = t.Type })
                                .ToList();
    }


    // NOTE: is case sensitive
    public IList<TitleSummaryDto> GetTitlesByName(string search, bool ordered = false)
    {
        if (!ordered)
        { 
            return _dbContext.Titles.Where(t => t.PrimaryTitle.Contains(search))
                                .Select(t => new TitleSummaryDto { PrimaryTitle = t.PrimaryTitle, StartYear = t.StartYear, Poster = t.Poster, Type = t.Type })
                                .ToList();
        }

        else
        {
            return _dbContext.Titles.Where(t => t.PrimaryTitle.Contains(search))
                                    .OrderBy(t => t.Id)
                                    .Select(t => new TitleSummaryDto { PrimaryTitle = t.PrimaryTitle, StartYear = t.StartYear, Poster = t.Poster, Type = t.Type })
                                    .ToList();
        }
    }





    //methods below are just for eplxoration
    public string? GetTitleNameById(string id) // only for testing
    {
        return _dbContext.Titles.FirstOrDefault(t => t.Id == id)?.PrimaryTitle;
    }

}
