using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;
using WebServiceLayer.DTOs;

namespace WebServiceLayer.Controllers;

public class BaseController: ControllerBase
{
    // protected fields and methods to allow access from derived classes
    protected LinkGenerator _linkGenerator;

    public BaseController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    

    protected object CreatePaging<T>(string endpointName, IEnumerable<T> items, int numberOfItems, PageSettings pageSettings, object? values = null)
    {

        // integer disivion rounding up (possible because numberOfItems and pageSettings.PageSize can not be negative)
        var numberOfPages = (numberOfItems + pageSettings.PageSize - 1) / pageSettings.PageSize;

        // ensure page is in valid range
        pageSettings.Page = Math.Clamp(pageSettings.Page, 0, Math.Max(0, numberOfPages - 1));

        // link to pages or null if there is no page
        RouteValueDictionary dict = new();

        if(values != null)
        {
            foreach(var prop in values.GetType().GetProperties())
            {
                dict.Add(prop.Name, prop.GetValue(values));
            }
        }


        dict.Add("page", pageSettings.Page - 1);
        dict.Add("pagesize", pageSettings.PageSize);
        var previous = pageSettings.Page > 0
            ? GetUrl(endpointName, dict)
            : null;
        dict["page"] = pageSettings.Page + 1;
        var next = pageSettings.Page < numberOfPages - 1
            ? GetUrl(endpointName, dict)
            : null;

        // links to first, current and last page
        dict["page"] = 0;
        var first = GetUrl(endpointName, dict);
        dict["page"] = pageSettings.Page;
        var current = GetUrl(endpointName, dict);
        dict["page"] = numberOfPages - 1;
        var last = GetUrl(endpointName, dict);

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

    protected string? GetUrl(string endpointName, RouteValueDictionary values)
    {
        return _linkGenerator.GetUriByName(HttpContext, endpointName, values);
    }
}
