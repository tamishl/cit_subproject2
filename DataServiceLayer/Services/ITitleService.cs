using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer.Services;

    public interface ITitleService
    {
    PagedResultDto<TitleSummaryDto> GetTitles(int page = 0, int pageSize = 10);
    PagedResultDto<TitleSummaryDto> GetTitlesByName(string search, int page = 0 , int pageSize = 10);
    PagedResultDto<TitleSummaryDto>? GetTitlesByGenre(string genreId, int page = 0, int pageSize = 10);
    public PagedResultDto<TitleSummaryDto> GetTitlesByType(string typeId, int page = 0, int pageSize = 10);

    //PagedResultDto<TitleAkaSummaryDto> GetAkas(string id, int page = 0, int pageSize = 10);
    TitleDto? GetTitle(string id);
    int GetTitleCount();





}
