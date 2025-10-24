using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;
using WebServiceLayer.Models;

namespace WebServiceLayer.Controllers;

public class BaseController: ControllerBase
{
    protected LinkGenerator _linkGenerator;

    public BaseController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }


    public object CreatePaging<T>(string endpointName, IEnumerable<T> items, int numberOfItems, PageSettings pageSettings)
    {

        //henriks version 
        //var numberOfPages = (int)Math.Ceiling((double)numberOfItems / pageSettings.PageSize); //conversion to double necesasry because Ceiling doesn't do int division
        
        var numberOfPages = (numberOfItems + pageSettings.PageSize - 1) / pageSettings.PageSize;

        var prev = pageSettings.Page > 0
            ? GetUrl(endpointName, new { page = pageSettings.Page - 1, pageSettings.PageSize })
            : null;

        var next = pageSettings.Page < numberOfPages - 1
            ? GetUrl(endpointName, new { page = pageSettings.Page + 1, pageSettings.PageSize })
            : null;

        var first = GetUrl(endpointName, new { page = 0, pageSettings.PageSize });
        var cur = GetUrl(endpointName, new { pageSettings.Page, pageSettings.PageSize });
        var last = GetUrl(endpointName, new { page = numberOfPages - 1, pageSettings.PageSize });

        return new
        {
            First = first,
            Prev = prev,
            Next = next,
            Last = last,
            Current = cur,
            NumberOfPages = numberOfPages,
            NumberOfIems = numberOfItems,
            Items = items
        };
    }

    protected string? GetUrl(string endpointName, object values)
    {
        return _linkGenerator.GetUriByName(HttpContext, endpointName, values);
    }
}
}
