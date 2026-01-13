using System.Text.Json.Serialization;

namespace Trivia.Models.DTO
{
    public class OpenTriviaTokenDTO
    {
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; set; }

        [JsonPropertyName("response_message")]
        public string? ResponseMessage { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }
}
