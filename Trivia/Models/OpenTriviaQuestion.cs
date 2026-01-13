using System.Text.Json.Serialization;

namespace Trivia.Models
{
    public class OpenTriviaQuestion
    {
        public string? Question { get; set; }
        public string[]? PossibleAnswers { get; set; }
    }
}
