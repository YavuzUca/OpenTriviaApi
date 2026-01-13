using System.Text.Json;
using Trivia.Models;

namespace Trivia.Services
{
    public class OpenTriviaBaseService
    {
        protected string? baseUrl;
        protected static readonly HttpClient httpClient = new HttpClient();

        public OpenTriviaBaseService()
        {
            baseUrl = "https://opentdb.com/";
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

