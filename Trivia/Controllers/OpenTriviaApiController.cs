using Microsoft.AspNetCore.Mvc;
using Trivia.Models;
using Trivia.Models.DTO;
using Trivia.Services;

namespace Trivia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenTriviaApiController(IOpenTriviaApiService apiService) : ControllerBase
    {
        private readonly IOpenTriviaApiService _apiService = apiService;

        [HttpGet("GetQuestions")]
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

        [HttpPost("CheckAnswer")]
        public bool CheckAnswer(string question, string answer)
        {
            return _apiService.CheckAnswer(question, answer);
        }
    }
}
