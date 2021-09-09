using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.AcceptanceTests.Helpers
{
    public static class HttpContentHelper
    {
        public static async Task<T> DeserializeAsync<T>(this HttpContent httpcontent)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await httpcontent.ReadAsStringAsync(), options);
        }
    }
}
