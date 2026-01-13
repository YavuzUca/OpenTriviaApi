using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trivia.Models.DTO
{
    public class OpenTriviaResultDTO
    {
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("difficulty")]
        public required string Difficulty { get; set; }

        [JsonPropertyName("category")]
        public required string Category { get; set; }

        [JsonPropertyName("question")]
        public required string Question { get; set; }

        [JsonPropertyName("correct_answer")]
        public required string CorrectAnswer { get; set; }

        [JsonPropertyName("incorrect_answers")]
        public required List<string> IncorrectAnswers { get; set; }
    }

    public class OpenTriviaResultsDTO
    {
        [JsonPropertyName("response_code")]
        public required int ResponseCode { get; set; }

        [JsonPropertyName("results")]
        public List<OpenTriviaResultDTO>? Results { get; set; }
    }
}
