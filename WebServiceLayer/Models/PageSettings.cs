namespace WebServiceLayer.Models;

public class PageSettings
{
    private const int MaxPageSize = 30;
    public int PageSize { get; set; } = 5;

    private int page = 1;
    public int Page
    {
        get { return page; }
        set
        { 
            page = value > MaxPageSize ? MaxPageSize : value; 
        }
    }
}
