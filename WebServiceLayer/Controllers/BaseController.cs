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

        // ensure page is in valid range
        pageSettings.Page = Math.Clamp(pageSettings.Page, 0, Math.Max(0, numberOfPages - 1));

        // link to pages or null if there is no page
        var previous = pageSettings.Page > 0
            ? GetUrl(endpointName, new { page = pageSettings.Page - 1, pageSettings.PageSize })
            : null;
        var next = pageSettings.Page < numberOfPages - 1
            ? GetUrl(endpointName, new { page = pageSettings.Page + 1, pageSettings.PageSize })
            : null;

        // links to first, current and last page
        var first = GetUrl(endpointName, new { page = 0, pageSettings.PageSize });
        var current = GetUrl(endpointName, new { pageSettings.Page, pageSettings.PageSize });
        var last = GetUrl(endpointName, new { page = numberOfPages - 1, pageSettings.PageSize });

        return new
        {
            First = first,
            Previous = previous,
            Next = next,
            Last = last,
            Current = current,
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
