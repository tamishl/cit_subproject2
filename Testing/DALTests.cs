namespace Testing;

public class DALTests
{
    [Fact]


    public void GetTitlesByName_ReturnsTitles()
    {
        var titleService = new DataServiceLayer.Services.TitleService();
        var titles = titleService.GetTitlesByName("Harry Potter", true);
        Assert.Equal(25, titles.Count);
        Assert.Equal("Lego Harry Potter and the Philosopher's Stone", titles[9].PrimaryTitle);
    }

    [Fact]
    public void GetTitles_ReturnsTitles()
    {
        var titleService = new DataServiceLayer.Services.TitleService();
        var titles = titleService.GetTitles(10);
        Assert.Equal(10, titles.Count);
        Assert.NotNull(titles[9]);
        Assert.NotEmpty(titles[6].PrimaryTitle);
    }
}
