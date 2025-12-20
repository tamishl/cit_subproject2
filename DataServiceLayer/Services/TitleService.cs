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

    public PagedResultDto<TitleSummaryDto> GetTitles(int page = 0, int pageSize = 10)
    {
        var query = _dbContext.Titles;
        var items = query.Include(t => t.TitleRating)
                         .OrderBy(t => t.Id)
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


    public PagedResultDto<TitleSummaryDto> GetTitlesBySearch(string search, int page = 0, int pageSize = 10, string title_type="all")
    {
        var query = _dbContext.TitleReadDtos.FromSqlInterpolated($"SELECT * FROM best_match_variadic_fieldrank({title_type}, VARIADIC {search.Split(' ')})");
        var items = query.Skip(page * pageSize)
                         .Take(pageSize)
                         .Select(t => t.Adapt<TitleSummaryDto>()).ToList();

        return new PagedResultDto<TitleSummaryDto>
        {
            Items = items,
            TotalNumberOfItems = query.Count()
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



    public PagedResultDto<TitleSummaryDto> GetTitlesByGenre(string genreId, int page = 0, int pageSize = 10)
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
                Id = c.PersonId,
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



    public int GetTitleCount()
    {
            return _dbContext.Titles.Count();
    }




    public TitleGrCastDto? GetTitleGroupedCast(string id)
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
            Writers = t.Writers == null ? null : t.Writers.Select(w => w.Name),
            Directors = t.Directors == null ? null : t.Directors.Select(d => d.Name),
            Cast = t.Cast == null ? null : t.Cast.GroupBy(c => c.PersonId)
                                                 .Select(gr => new PersonGroupedCastDto { 
                                                              PersonId = gr.Key, // Key = what it's grouped by 
                                                              ProfessionIds = gr.Where(p => p.Profession != null)
                                                                                .Select(p => p.Profession.Id)
                                                                                .Distinct()
                                                                                .ToList(),
                                                             CharacterNames = gr.Where(p => p.CharacterName != null)
                                                                                .Select(p => p.CharacterName)
                                                                                .ToList()
                                                 }).ToList(),
            TitleRating = t.TitleRating == null ? null : new TitleRatingDto
            {
                TitleId = t.TitleRating.TitleId,
                AverageRating = t.TitleRating.AverageRating,
                Votes = t.TitleRating.Votes
            }
        })
                                  .FirstOrDefault(t => t.Id == id).Adapt<TitleGrCastDto>();
    }



    //private static PersonGroupedCastDto GroupedCast(IQueryable<Casting> castings)
    //{ 
    //    => castings.
    //}

    //private static PersonGroupedCastDto? GroupCasting(ICollection<Casting> castings)
    //{
    //    ICollection<PersonGroupedCastDto> grouped = [];
    //    List<string> ids = [];

    //    foreach(Casting c in castings)
    //    {
    //        if (!ids.Contains(c.PersonId))
    //        {
    //            grouped.Add(new PersonGroupedCastDto
    //            {
    //                PersonId = c.PersonId,
    //                ProfessionIds = c.Profession.Id == null ? null : [c.Profession.Id],
    //                CharacterNames = c.CharacterName == null ? null : [c.CharacterName]
    //            });
    //            ids.Add(c.PersonId);
    //        }

    //        else if (ids.Contains(c.PersonId))
    //        {
    //            foreach (PersonGroupedCastDto g in grouped)
    //            {
    //                if (g.PersonId == c.PersonId)
    //                {
    //                    if (c.Profession.Id != null)
    //                    {
    //                        if (g.ProfessionIds != null)
    //                        {
    //                            g.ProfessionIds.Add(c.Profession.Id);
    //                        }
    //                        else
    //                        {
    //                            g.ProfessionIds = [c.Profession.Id];
    //                        }
    //                    }
    //                    if (c.CharacterName != null)
    //                    {
    //                        if (g.CharacterNames != null)
    //                        {
    //                            g.CharacterNames.Add(c.CharacterName);
    //                        }
    //                        else
    //                        {
    //                            g.CharacterNames = [c.CharacterName];
    //                        }
    //                    }
    //                    break;
    //                }
    //                else
    //                { 
    //                    continue;
    //                }
    //            }            
    //        }
    //    }

    //    return null;
    //}
}
