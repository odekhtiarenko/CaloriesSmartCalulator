using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.AcceptanceTests.Helpers
{
    public static class HttpClientHelper
    {
        public static async Task<HttpResponseMessage> GetWithExpectedStatusCodeAsync(this HttpClient httpClient, string url, HttpStatusCode expectedStatusCode)
        {
            var response = await httpClient.GetAsync(url);
            response.StatusCode.Should().Be(expectedStatusCode);

            return response;
        }

        public static async Task<HttpResponseMessage> PostWithExpectedStatusCodeAsync<T>(this HttpClient httpClient, string url, T content, HttpStatusCode expectedStatusCode)
        {
            var response = await httpClient.PostAsync(url, JsonContent.Create(content));

            response.StatusCode.Should().Be(expectedStatusCode);

            return response;
        }
    }
}
