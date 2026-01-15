using Microsoft.AspNetCore.Mvc;
using Trivia.Models;
using Trivia.Models.DTO;
using Trivia.Services;

namespace Trivia.Controllers
{
    [ApiController]
    [Route("api/")]
    public class OpenTriviaApiController(IOpenTriviaApiService apiService) : ControllerBase
    {
        private readonly IOpenTriviaApiService _apiService = apiService;

        [HttpGet("getQuestions")]
        public async Task<ActionResult<List<OpenTriviaQuestion>>> GetQuestions(
            int? category = null,
            string? difficulty = null,
            string? type = null,
            string? token = null,
            int amount = 10)
        {
            var (code, questions) = await _apiService.GetQuestionsAsync(category, difficulty, type, token, amount);

            return code switch
            {
                ResponseCode.Success => Ok(questions),
                ResponseCode.NoResults => NotFound(),
                ResponseCode.Invalid => BadRequest("Invalid parameters."),
                ResponseCode.TokenNotFound => BadRequest("The token used has not been found."),
                ResponseCode.TokenEmpty => BadRequest("The token used has not been found."),
                ResponseCode.RateLimit => BadRequest("Rate limit has been reached."),
                _ => StatusCode(502)
            };
        }

        [HttpPost("checkAnswerWithQuestion")]
        public ActionResult<OpenTriviaQuestionAnswer> CheckAnswer(string question, string answer)
        {
            var result = _apiService.CheckSingleAnswer(question, answer);
            return Ok(result);
        }

        [HttpPost("checkAnswerWithId")]
        public ActionResult<OpenTriviaQuestionAnswer> CheckAnswer(int questionId, string answer)
        {
            var result = _apiService.CheckSingleAnswer(questionId, answer);
            return Ok(result);
        }

        [HttpPost("checkMultipleAnswers")]
        public ActionResult<List<OpenTriviaQuestionAnswer>> CheckAnswer([FromBody] Dictionary<int, string> questionAnswers)
        {
            var results = _apiService.CheckMultipleAnswers(questionAnswers);
            return Ok(results);
        }
    }
}
