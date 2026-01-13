using System.Net.Http;
using System.Text.Json;
using Trivia.Models;
using Trivia.Models.DTO;

namespace Trivia.Services
{
    public interface IOpenTriviaTokenService
    {
        Task<OpenTriviaTokenDTO> RequestSessionToken();
        Task<OpenTriviaTokenDTO> ResetSessionToken(string sessionToken);
        bool IsValidToken(string sessionToken);
    }
    public class OpenTriviaTokenService : OpenTriviaBaseService, IOpenTriviaTokenService
    {
        public OpenTriviaTokenService() { }

        public async Task<OpenTriviaTokenDTO> RequestSessionToken()
        {
            string endpoint = baseUrl + "api_token.php?command=request";
            return await GenericOpenTriviaGetJSON<OpenTriviaTokenDTO>(endpoint);
        }

        public async Task<OpenTriviaTokenDTO> ResetSessionToken(string sessionToken)
        {
            string endpoint = baseUrl + "api_token.php?command=reset&token=" + sessionToken;
            return await GenericOpenTriviaGetJSON<OpenTriviaTokenDTO>(endpoint);
        }

        public bool IsValidToken(string sessionToken)
        {
            var expectedLength = 64;

            if (string.IsNullOrWhiteSpace(sessionToken) || sessionToken.Length != expectedLength)
                return false;

            return sessionToken.All(Uri.IsHexDigit);
        }
    }
}
