using Newtonsoft.Json;
using System.Text;

namespace UnitTest.Helper
{
    public partial class ApiHelper : IApiHelper
    {
        private readonly HttpClient _httpClient;
        JsonSerializerSettings settings;

        public ApiHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
            settings = new JsonSerializerSettings();
        }

        public virtual async Task<(T?, int)> BaseApiCall<T>(string url, object obj, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, url);
            var json = JsonConvert.SerializeObject(obj, settings);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _httpClient
                  .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                  .ConfigureAwait(false);

            try
            {
                var content = await response.Content.ReadAsStringAsync();
                return (JsonConvert.DeserializeObject<T>(content), (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return (default(T),(int)response.StatusCode);
            }

        }
    }
}
