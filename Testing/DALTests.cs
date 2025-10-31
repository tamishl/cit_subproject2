namespace Testing;

public class DALTests
{
    [Fact]
    public void GetTitles_Valid_ReturnsTitles()
    {
        var titleService = new DataServiceLayer.Services.TitleService();
        var result = titleService.GetTitles(10);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(158999, result.TotalNumberOfItems);
        Assert.NotNull(result.Items[9]);
        Assert.NotEmpty(result.Items[6].PrimaryTitle);

    }

    [Fact]
    public void GetTitlesByName_Valid_ReturnsTitles()
    {
        var titleService = new DataServiceLayer.Services.TitleService();
        var result = titleService.GetTitlesByName("Harry Potter", 0, 100);
        Assert.Equal(25, result.Items.Count);
        Assert.Equal("Lego Harry Potter and the Philosopher's Stone", result.Items[9].PrimaryTitle);
    }


    [Fact]
    public void GetTitlesByName_InValid_ReturnsNoTitles()
    {
        var titleService = new DataServiceLayer.Services.TitleService();
        var result = titleService.GetTitlesByName("sijneqlwehbq", 0, 100);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalNumberOfItems);
    }




    [Fact]
    public void GetTitlesByGenre_Valid_ReturnsTitles()
    {
        var titleService = new DataServiceLayer.Services.TitleService();
        var result = titleService.GetTitlesByGenre("horror", 0, 20);
        Assert.Equal(20, result.Items.Count);
        Assert.Equal(3998, result.TotalNumberOfItems);
    }




}
