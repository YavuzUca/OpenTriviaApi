using System.Text.Json.Serialization;

namespace Trivia.Models
{
    public class OpenTriviaQuestion
    {
        public int Id { get; set; }
        public string? Question { get; set; }
        public string[]? PossibleAnswers { get; set; }
    }
}
