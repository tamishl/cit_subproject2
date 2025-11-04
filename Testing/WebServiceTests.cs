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



    [Fact]
    public void TitlesApi_GetWithNoArguments_OkAndTenTitles()
    {
        var (data, statusCode) = GetArray(TitlesApi);

        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.Equal(8, data.Count);
        Assert.Equal("Beverages", data.First()["name"]);
        Assert.Equal("Seafood", data.Last()["name"]);
    }



    // Helper methods

    (JObject, HttpStatusCode) GetObject(string url)
    {
        var client = new HttpClient();
        var response = client.GetAsync(url).Result; //HttpResponseMessage: statuscode, headers and content
        var data = response.Content.ReadAsStringAsync().Result;
        return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
    }

}
