using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers;

public class BaseController: ControllerBase
{
    // protected fields and methods to allow access from derived classes
    protected LinkGenerator _linkGenerator;

    public BaseController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }


    protected object CreatePaging<T>(string endpointName, IEnumerable<T> items, int numberOfItems, PageSettings pageSettings)
    {
        // integer disivion rounding up (possible because numberOfItems and pageSettings.PageSize can not be negative)
        var numberOfPages = (numberOfItems + pageSettings.PageSize - 1) / pageSettings.PageSize;

        // link to pages or null if there is no page
        var previous = pageSettings.Page > 1
            ? GetUrl(endpointName, new { page = pageSettings.Page - 1, pageSettings.PageSize })
            : null;
        var next = pageSettings.Page < numberOfPages - 1
            ? GetUrl(endpointName, new { page = pageSettings.Page + 1, pageSettings.PageSize })
            : null;

        // links to first, current and last page
        var first = GetUrl(endpointName, new { page = 1, pageSettings.PageSize });
        var cur = GetUrl(endpointName, new { pageSettings.Page, pageSettings.PageSize });
        var last = GetUrl(endpointName, new { page = numberOfPages, pageSettings.PageSize });

        return new
        {
            First = first,
            Previous = previous,
            Next = next,
            Last = last,
            Current = cur,
            NumberOfPages = numberOfPages,
            NumberOfItems = numberOfItems,
            Items = items
        };
    }


    // values of type object to allow anonymous types
    protected string? GetUrl(string endpointName, object values)
    {
        return _linkGenerator.GetUriByName(HttpContext, endpointName, values);
    }
}
