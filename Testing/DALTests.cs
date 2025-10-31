namespace Testing;

public class DALTests
{
    [Fact]


    public void GetTitlesByName_ReturnsTitles()
    {
        var titleService = new DataServiceLayer.Services.TitleService();
        var result = titleService.GetTitlesByName("Harry Potter");
        Assert.Equal(25, result.Items.Count);
        Assert.Equal("Lego Harry Potter and the Philosopher's Stone", result.Items[9].PrimaryTitle);
    }

    [Fact]
    public void GetTitles_ReturnsTitles()
    {
        var titleService = new DataServiceLayer.Services.TitleService();
        var result = titleService.GetTitles(10);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(158999, result.TotalNumberOfItems);
        Assert.NotNull(result.Items[9]);
        Assert.NotEmpty(result.Items[6].PrimaryTitle);

    }
}
