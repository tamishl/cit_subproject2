using DataServiceLayer.Domains;
using DataServiceLayer.Services;

namespace Testing;

public class DataLayerTests
{
    /////////////////////////////////////////////////////////
    ///                       TITLE                       ///                   
    /////////////////////////////////////////////////////////
    [Fact]
    public void GetTitles_Valid_ReturnsTitles()
    {
        var titleService = new TitleService();
        var result = titleService.GetTitles(10);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(158999, result.TotalNumberOfItems);
        Assert.NotNull(result.Items[9]);
        Assert.NotEmpty(result.Items[6].PrimaryTitle);

    }

    [Fact]
    public void GetTitlesByName_Valid_ReturnsTitles()
    {
        var titleService = new TitleService();
        var result = titleService.GetTitlesByName("Harry Potter", 0, 100);
        Assert.Equal(25, result.Items.Count);
        Assert.Equal("Lego Harry Potter and the Philosopher's Stone", result.Items[9].PrimaryTitle);
    }


    [Fact]
    public void GetTitlesByName_InValid_ReturnsNoTitles()
    {
        var titleService = new TitleService();
        var result = titleService.GetTitlesByName("sijneqlwehbq", 0, 100);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalNumberOfItems);
    }



    [Fact]
    public void GetTitlesByGenre_Valid_ReturnsTitles()
    {
        var titleService = new TitleService();
        var result = titleService.GetTitlesByGenre("horror", 0, 20);
        Assert.Equal(20, result.Items.Count);
        Assert.Equal(3998, result.TotalNumberOfItems);
    }


    [Fact]
    public void GetTitle_Valid_ReturnsTitle()
    {
        var titleService = new TitleService();
        var result = titleService.GetTitle("tt7311010");
        Assert.Equal("Returning the Favor", result.PrimaryTitle);
        Assert.Equal("2017", result.StartYear);
        Assert.NotNull(result.Poster);
        Assert.Equal(462, result.TitleRating.Votes);
        Assert.Equal(8.5, result.TitleRating.AverageRating);
        Assert.Equal(24, result.Cast.Count);
        Assert.Contains("Adventure", result.Genres);
    }

    [Fact]
    public void GetTitle_InValid_ReturnsNull()
    {
        var titleService = new TitleService();
        var result = titleService.GetTitle("ttmadeup");
        Assert.Null(result);
    }




    /////////////////////////////////////////////////////////
    ///                        PERSON                     ///                   
    /////////////////////////////////////////////////////////

    [Fact]
    public void GetPerson_Valid_ReturnsPersonDetails()
    {
        var personService = new PersonService();
        var result = personService.GetPerson("nm0000138");

        Assert.NotNull(result);
        Assert.Equal("Leonardo DiCaprio", result.Name);
        Assert.Equal("1974", result.BirthYear);
        Assert.NotNull(result.KnownForTitles);
        Assert.Contains("Inception", result.KnownForTitles);
    }

    [Fact]
    public void GetPersonsByName_Valid_ReturnsPersons()
    {
        var personService = new PersonService();
        var result = personService.GetPersonsByName("Leonardo", 0, 10);

        Assert.NotNull(result);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(198, result.TotalNumberOfItems);
        Assert.Equal("Leonardo DiCaprio", result.Items[0].Name);
    }

    [Fact]
    public void GetPeopleByName_InValid_ReturnsNoPeople()
    {
        var personService = new PersonService();
        var result = personService.GetPersonsByName("aaaaaaaa", 0, 10);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalNumberOfItems);
    }




    /////////////////////////////////////////////////////////
    ///                       TITLE                       ///                   
    /////////////////////////////////////////////////////////




    /////////////////////////////////////////////////////////
    ///                         USER                      ///                   
    /////////////////////////////////////////////////////////
    [Fact]
    public void CreateUser_Valid_CreatesAndReturnsNewUser()
    {
        var userService = new UserService();
        var newUser = userService.CreateUser(username: "Blommo",
                                             firstName: null,
                                             lastName: null,
                                             email: "minMail@hotmail.com",
                                             password: "etpassword",
                                             salt: "saltmedmeresalt");
        Assert.Equal("Blommo", newUser.Username);
        Assert.NotNull(newUser.Email);

        userService.DeleteUser(newUser);
    }

    
    [Fact]
    public void GetUser_Valid_ReturnsUser()
    {
        var userService = new UserService();
        userService.CreateUser(username: "Blommo",
                               email: "minMail@hotmail.com",
                               firstName: null,
                               lastName: null,
                               password: "etpassword",
                               salt: "saltmedmeresalt");

        var user = userService.GetUser("Blommo");
        Assert.Equal("minMail@hotmail.com", user.Email);
        Assert.NotNull(user.Password);

        userService.DeleteUser(user);
    }

    [Fact]
    public void GetAllUsers_Valid_ReturnsAListOfUserMinimumDetialsDto()
    {
        var userService = new UserService();
        userService.CreateUser(username: "Jalte",
                              email: "Har@hotmail.com",
                              firstName: null,
                              lastName: null,
                              password: "etpassword",
                              salt: "saltmedmeresalt");

        userService.CreateUser(username: "Blommo",
                              email: "minMail@hotmail.com",
                              firstName: null,
                              lastName: null,
                              password: "etpassword",
                              salt: "saltmedmeresaltmedendnumeresalt");

        var users = userService.GetAllUsers();
        Assert.Equal(2, users.Items.Count);
        Assert.Equal("Jalte", users.Items[1].Username);

        userService.DeleteUser("Blommo");
        userService.DeleteUser("Jalte");
    }

    [Fact]
    public void UpdateUser_Valid_UpdatesUserAndReturnsTrue()
    {
        var userService = new UserService();

        // Arrange: create a user
        var user = userService.CreateUser(username: "Blommo",
                                          email: "minMail@hotmail.com",
                                          firstName: null,
                                          lastName: null,
                                          password: "etpassword",
                                          salt: "saltmedmeresaltmedendnumeresalt"
        );


        user.Email = "updated@example.com";
        user.FirstName = "UpdatedName";

        var result = userService.UpdateUser(user);

        Assert.True(result);

        var updatedUser = userService.GetUser("Blommo");
        Assert.NotNull(updatedUser);
        Assert.Equal("updated@example.com", updatedUser.Email);
        Assert.Equal("UpdatedName", updatedUser.FirstName);

        // Cleanup
        userService.DeleteUser("Blommo");
    }

    [Fact]
    public void DeleteUser_Valid_DeletesUserAndReturnsTrue()
    {
        var userService = new UserService();

        // Arrange: create a user
        userService.CreateUser(username: "Blommo",
                               email: "minMail@hotmail.com",
                               firstName: null,
                               lastName: null,
                               password: "etpassword",
                               salt: "saltmedmeresaltmedendnumeresalt"
        );
       

        // Act: delete the user
        var result = userService.DeleteUser("Blommo");

        // Assert
        Assert.True(result);

        var deletedUser = userService.GetUser("Blommo");
        Assert.Null(deletedUser);
    }

    [Fact]
    public void DeleteUser_NonExistent_ReturnsFalse()
    {
        var userService = new UserService();

        // Act: try to delete a user that doesn't exist
        var result = userService.DeleteUser("ThePhantom");

        // Assert
        Assert.False(result);
    }





}
