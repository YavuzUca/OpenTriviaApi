using Microsoft.AspNetCore.Mvc;
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
        OpenTriviaQuestionAnswer CheckSingleAnswer(string question, string answer);
        OpenTriviaQuestionAnswer CheckSingleAnswer(int questionId, string answer);
        List<OpenTriviaQuestionAnswer> CheckMultipleAnswers(Dictionary<int, string> questionAnswers);
    }

    public class OpenTriviaApiService : OpenTriviaBaseService, IOpenTriviaApiService
    {
        private readonly ILogger<OpenTriviaApiService> Logger;
        private readonly OpenTriviaContext DbContext;

        public OpenTriviaApiService(
            ILogger<OpenTriviaApiService> logger, 
            OpenTriviaContext context,
            HttpClient httpClient
            ) : base(httpClient)
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
            string endpoint = httpClient.BaseAddress + "api.php?" + $"amount={amount}";

            if (category != null)
                endpoint += $"&category={category}";

            if (difficulty != null)
                endpoint += $"&difficulty={difficulty}";

            if (type != null)
                endpoint += $"&type={type}";

            if (sessionToken != null)
                endpoint += $"&token={sessionToken}";

            return await GenericOpenTriviaGetJSON<OpenTriviaResultsDTO>(endpoint);
        }

        public List<OpenTriviaQuestion> GetQuestions()
        {
            var list = new List<OpenTriviaQuestion>();

            foreach(var result in DbContext.OpenTriviaResults)
            {
                var question = new OpenTriviaQuestion
                {
                    Id = result.Id,
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
                DbContext.OpenTriviaResults.RemoveRange(DbContext.OpenTriviaResults);
                DbContext.OpenTriviaResults.AddRange(apiResponse.Results!);
                DbContext.SaveChanges();
            }

            return (responseCode, GetQuestions());
        }

        public OpenTriviaQuestionAnswer CheckSingleAnswer(string question, string answer)
        {
            var findQuestion = DbContext.OpenTriviaResults.FirstOrDefault(i => i.Question == question);
            var questionAnswer = new OpenTriviaQuestionAnswer
            {
                Id = findQuestion != null ? findQuestion.Id : 0,
                Question = findQuestion != null ? findQuestion.Question : "",
                IsCorrectAnswer = (findQuestion != null && findQuestion.CorrectAnswer == answer).ToString()
            };

            return questionAnswer;
        }
        public OpenTriviaQuestionAnswer CheckSingleAnswer([FromQuery] int questionId, [FromQuery] string answer)
        {
            var findQuestion = DbContext.OpenTriviaResults.FirstOrDefault(i => i.Id == questionId);
            var questionAnswer = new OpenTriviaQuestionAnswer
            {
                Id = findQuestion != null ? findQuestion.Id : 0,
                Question = findQuestion != null ? findQuestion.Question : "",
                IsCorrectAnswer = (findQuestion != null && findQuestion.CorrectAnswer == answer).ToString()
            };

            return questionAnswer;
        }

        public List<OpenTriviaQuestionAnswer> CheckMultipleAnswers(Dictionary<int, string> questionAnswers)
        {
            var results = new List<OpenTriviaQuestionAnswer>();

            foreach (var qa in questionAnswers)
            {
                var result = CheckSingleAnswer(qa.Key, qa.Value);
                results.Add(result);
            }

            return results;
        }   
    }
}
