using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceLayer.Domains;
using DataServiceLayer.DTOs;

namespace DataServiceLayer.Services;

    public interface ITitleService
    {
    PagedResultDto<TitleSummaryDto> GetTitles(int page = 0, int pageSize = 10, bool includeCount = true);
    PagedResultDto<TitleSummaryDto> GetTitlesByName(string search, int page = 0 , int pageSize = 10, bool includeCount = true);
    //PagedResultDto<TitleSummaryDto> GetTitlesByName(string search, int page = 1 , int pageSize = 10, bool includeCount = true);

    int GetTitleCount();





}
