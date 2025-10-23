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


    public IList<TitleSummaryDto> GetTitlesByName(string search)
    {
        return _dbContext.Titles.Where(t => t.PrimaryTitle.Contains(search))
                                .Select(t => new TitleSummaryDto { PrimaryTitle = t.PrimaryTitle, StartYear = t.StartYear, Poster = t.Poster, Type=t.Type})
                                .ToList();
    }





    //methods below are just for eplxoration
    public IList<Title> GetTitles(int max = 10)
    {
        return _dbContext.Titles.Take(max) // limit
                                .ToList();
    }
    public string? GetTitleNameById(string id) // only for testing
    {
        return _dbContext.Titles.FirstOrDefault(t => t.Id == id)?.PrimaryTitle;
    }


    // NOTE: is case sensitive
    public string? GetTitleNameByName(string search)
    {
        return _dbContext.Titles.FirstOrDefault(t => t.PrimaryTitle.Contains(search))?.PrimaryTitle;
    }
}
