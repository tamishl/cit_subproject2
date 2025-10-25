namespace WebServiceLayer.Models;

public class PageSettings
{   public int PageSize { get; set; } = 10;

    private int _page = 0;
    public int Page
    {
        get { return _page; }
        set
        {
            _page = value < 0 ? 0 : value;

            // this check can not be done here because NumberOfPages is not known yet
            // ensure page is at least 1 or maximum MaxPageSize
            //_page = Math.Clamp(value, 0, NumberOfPages-1);
        }
    }
}
