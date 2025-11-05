using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Testing;

public class WebServiceTests

{
    private const string TitlesApi = "https://localhost:5555/api/titles";
    private readonly HttpClient _client = new HttpClient();


    /////////////////////////////////////////////////////////
    ///                        DATA                       ///                   
    /////////////////////////////////////////////////////////


    [Fact]
    public void TitlesApi_GetWithNoArguments_OkAndTitles()
    {
        var (data, statusCode) = GetArray(TitlesApi);

        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.Equal(10, data.Count);
        Assert.Equal("The Twilight Zone", data.First()["primaryTitle"]);
    }


    //public void TitlesApi_GetTitleValidArgs_OkAndTitle()
    //{
    //    string title = "tt37976775";
    //    var (data, statusCode) = GetObject($"{TitlesApi}/")

    //}



    //[Fact]
    //public void RatingsApi_GetRatings()
    //{
    //    var (data, statusCode) = GetArray(RatingsApi);

    //    Assert.Equal(HttpStatusCode.OK, statusCode);
    //    Assert.Equal(10, data.Count);
    //    Assert.Equal("The Twilight Zone", data.First()["primaryTitle"]);
    //}












    // Helper methods
    // Debug line: //Console.WriteLine($"---DATA IN <method>{data}---");

    (JArray, HttpStatusCode) GetArray(string url)
    {
        

        // HttpResponseMessage: statuscode, headers and content
        var response = _client.GetAsync(url).Result;

        // Read body as string: raw JSON text
        var result = response.Content.ReadAsStringAsync().Result;

        // Convert JSON string to JObject so it has an indexer (plain objects don't)
        // Cast to JSON Array
        var jObj = JsonConvert.DeserializeObject<JObject>(result);
        var data = (JArray)jObj["items"];


        return (data, response.StatusCode);
    }



    (JObject, HttpStatusCode) GetObject(string url)
    {

        // HttpResponseMessage: statuscode, headers and content
        var response = _client.GetAsync(url).Result;

        // Read body as string: raw JSON text
        var data = response.Content.ReadAsStringAsync().Result;

        // Convert JSON string to .NET object
        // Cast .NET object into a JSON object
        // Add status code
        return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
    }
}
