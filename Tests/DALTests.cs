using Xunit;

namespace Tests;
public class DALTests
{
    [Fact]
    public void GetTitlesByName_Returns_Titles()
    {
        var titleService = new DataServiceLayer.Services.TitleService();
        var titles = titleService.GetTitlesByName("Harry Potter");
        Assert.Equal(25, titles.Count);
    }
}

