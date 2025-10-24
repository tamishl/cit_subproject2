using Microsoft.AspNetCore.Mvc;

namespace WebServiceLayer.Controllers;

public class BaseController: ControllerBase
{
    protected LinkGenerator _linkGenerator;

    public BaseController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }


    //public object CreatePaging<T>(string endPointName, IEnumerable<T> items,   )
}
