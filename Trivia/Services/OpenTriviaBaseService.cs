using System.Text.Json;
using Trivia.Models;

namespace Trivia.Services
{
    public class OpenTriviaBaseService
    {
        protected readonly HttpClient httpClient;

        public OpenTriviaBaseService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        protected async Task<T> GenericOpenTriviaGetJSON<T>(string endpoint)
        {
            var response = await httpClient.GetAsync(endpoint);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(json)!;

            return result;
        }
    }
}

