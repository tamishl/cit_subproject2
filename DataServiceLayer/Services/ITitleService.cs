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
    IList<TitleSummaryDto> GetTitlesByName(string search, bool ordered = false);
    IList<TitleSummaryDto> GetTitles(int max = 10);





}
