using BlazorTestClean.Models;
using BlazorTestClean.Services.Interfaces;
using System.Text.Json;

namespace BlazorTestClean.Services
{
    public class ApiService: IApiService
    {
        private readonly HttpClient _http;

        public ApiService(HttpClient http)
        {
            _http = http;
        }
        public async Task<Port> GetFleet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"{_http.BaseAddress}fleets/random"));
            request.Headers.Add("Accept", "application/json");

            var response =  await _http.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var currentFleet = JsonSerializer.Deserialize<Port>(content, options);

            return currentFleet ?? new Port();
        }
    }
}
