using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Trivia.Models;
using Trivia.Models.DTO;

namespace Trivia.Services
{
    public interface IOpenTriviaApiService
    {
        Task<OpenTriviaResultsDTO> GetApiResult(int? category = null, string? difficulty = null, string? type = null, string? sessionToken = null, int amount = 10);
        List<OpenTriviaQuestion> GetQuestions();
        Task<(ResponseCode Code, List<OpenTriviaQuestion>? Questions)> GetQuestionsAsync(int? category = null, string? difficulty = null, string? type = null, string? sessionToken = null, int amount = 10);
        bool CheckAnswer(string question, string answer);
    }

    public class OpenTriviaApiService : OpenTriviaBaseService, IOpenTriviaApiService
    {
        private readonly ILogger<OpenTriviaApiService> Logger;
        private readonly OpenTriviaContext DbContext;

        public OpenTriviaApiService(ILogger<OpenTriviaApiService> logger, OpenTriviaContext context)
        {
            Logger = logger;
            DbContext = context;
        }

        public async Task<OpenTriviaResultsDTO> GetApiResult(
            int? category = null, 
            string? difficulty = null, 
            string? type = null, 
            string? sessionToken = null,
            int amount = 10)
        {
            string endpoint = baseUrl + "api.php?" + $"amount={amount}";

            if (category != null)
                endpoint += $"category={category}";

            if (difficulty != null)
                endpoint += $"difficulty={difficulty}";

            if (type != null)
                endpoint += $"type={type}";

            if (sessionToken != null)
                endpoint += $"token={sessionToken}";

            return await GenericOpenTriviaGetJSON<OpenTriviaResultsDTO>(endpoint);
        }

        public List<OpenTriviaQuestion> GetQuestions()
        {
            var list = new List<OpenTriviaQuestion>();

            foreach(var result in DbContext.OpenTriviaResults)
            {
                var question = new OpenTriviaQuestion
                {
                    Question = result.Question,
                    PossibleAnswers = [result.CorrectAnswer, .. result.IncorrectAnswers]
                };

                list.Add(question);

            }

            return list;
        }

        public async Task<(ResponseCode Code, List<OpenTriviaQuestion>? Questions)> GetQuestionsAsync(
            int? category = null,
            string? difficulty = null,
            string? type = null,
            string? sessionToken = null,
            int amount = 10)
        {
            var apiResponse = await GetApiResult(category, difficulty, type, sessionToken, amount);
            var responseCode = (ResponseCode)apiResponse.ResponseCode;

            if (apiResponse != null && ResponseCode.Success == (ResponseCode)apiResponse.ResponseCode)
            {
                DbContext.OpenTriviaResults.AddRange(apiResponse.Results!);
                DbContext.SaveChanges();
            }

            return (responseCode, GetQuestions());
        }

        public bool CheckAnswer(string question, string answer)
        {
            var findQuestion = DbContext.OpenTriviaResults.FirstOrDefault(i => i.Question == question);

            if (findQuestion == null) 
                return false;

            return findQuestion.CorrectAnswer == answer;
        }
    }
}
