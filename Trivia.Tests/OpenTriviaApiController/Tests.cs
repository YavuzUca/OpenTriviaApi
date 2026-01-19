using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Trivia.Models;
using Trivia.Models.DTO;          
using Trivia.Services;
using Trivia.Tests.OpenTriviaApiController;
using Xunit;

namespace Trivia.Tests.Services
{
    public class OpenTriviaApiServiceTests : IClassFixture<QuestionsSeedDataFixture>
    {
        private QuestionsSeedDataFixture Fixture;

        public OpenTriviaApiServiceTests(QuestionsSeedDataFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public void GetQuestions_WhenDbHasResults_ReturnsMappedQuestionsWithPossibleAnswers()
        {
            // Arrange
            using var ctx = Fixture.CreateDbContext();

            ctx.OpenTriviaResults.AddRange(
                new OpenTriviaResultDTO
                {
                    Id = 1,
                    Question = "Q1",
                    Type = "test",
                    Difficulty = "test",
                    Category = "test",
                    CorrectAnswer = "A",
                    IncorrectAnswers = [ "B", "C", "D" ]
                },
                new OpenTriviaResultDTO
                {
                    Id = 2,
                    Question = "Q2",
                    Type = "test",
                    Difficulty = "test",
                    Category = "test",
                    CorrectAnswer = "True",
                    IncorrectAnswers = ["False"]
                }
            );
            ctx.SaveChanges();

            var service = CreateService(ctx);

            // Act
            var result = service.GetQuestions();

            // Assert
            result.Should().HaveCount(2);

            result[0].Id.Should().Be(1);
            result[0].Question.Should().Be("Q1");
            result[0].PossibleAnswers.Should().NotBeNull();
            result[0].PossibleAnswers!.Should().Contain([ "A", "B", "C", "D" ]);
            result[0].PossibleAnswers![0].Should().Be("A"); 

            result[1].Id.Should().Be(2);
            result[1].PossibleAnswers!.Should().Contain([ "True", "False" ]);

            ctx.Database.EnsureDeleted();
        }

        [Fact]
        public void CheckSingleAnswer_ByQuestionText_WhenCorrectAnswer_ReturnsTrue()
        {
            // Arrange
            using var ctx = Fixture.CreateDbContext();
            ctx.OpenTriviaResults.Add(new OpenTriviaResultDTO
            {
                Id = 10,
                Question = "Capital of NL?",
                Type = "test",
                Difficulty = "test",
                Category = "test",
                CorrectAnswer = "Amsterdam",
                IncorrectAnswers = [ "Rotterdam", "Utrecht", "Eindhoven" ]
            });
            ctx.SaveChanges();

            var service = CreateService(ctx);

            // Act
            var result = service.CheckSingleAnswer("Capital of NL?", "Amsterdam");

            // Assert
            result.Id.Should().Be(10);
            result.Question.Should().Be("Capital of NL?");
            result.IsCorrectAnswer.Should().Be("True");

            ctx.Database.EnsureDeleted();
        }

        [Fact]
        public void CheckSingleAnswer_ByQuestionText_WhenWrongAnswer_ReturnsFalse()
        {
            // Arrange
            using var ctx = Fixture.CreateDbContext();
            ctx.OpenTriviaResults.Add(new OpenTriviaResultDTO
            {
                Id = 11,
                Question = "2+2?",
                Type = "test",
                Difficulty = "test",
                Category = "test",
                CorrectAnswer = "4",
                IncorrectAnswers = [ "3", "5", "22" ]
            });
            ctx.SaveChanges();

            var service = CreateService(ctx);

            // Act
            var result = service.CheckSingleAnswer("2+2?", "5");

            // Assert
            result.Id.Should().Be(11);
            result.IsCorrectAnswer.Should().Be("False");

            ctx.Database.EnsureDeleted();
        }

        [Fact]
        public void CheckSingleAnswer_ByQuestionText_WhenQuestionNotFound_ReturnsDefaultsAndFalse()
        {
            // Arrange
            using var ctx = Fixture.CreateDbContext();
            var service = CreateService(ctx);

            // Act
            var result = service.CheckSingleAnswer("does-not-exist", "anything");

            // Assert
            result.Id.Should().Be(0);
            result.Question.Should().Be("");
            result.IsCorrectAnswer.Should().Be("False");

            ctx.Database.EnsureDeleted();
        }

        [Fact]
        public void CheckSingleAnswer_ById_WhenCorrectAnswer_ReturnsTrue()
        {
            // Arrange
            using var ctx = Fixture.CreateDbContext();
            ctx.OpenTriviaResults.Add(new OpenTriviaResultDTO
            {
                Id = 42,
                Question = "Sky color?",
                Type = "test",
                Difficulty = "test",
                Category = "test",
                CorrectAnswer = "Blue",
                IncorrectAnswers = [ "Green", "Red", "Yellow" ]
            });
            ctx.SaveChanges();

            var service = CreateService(ctx);

            // Act
            var result = service.CheckSingleAnswer(42, "Blue");

            // Assert
            result.Id.Should().Be(42);
            result.Question.Should().Be("Sky color?");
            result.IsCorrectAnswer.Should().Be("True");

            ctx.Database.EnsureDeleted();
        }

        [Fact]
        public void CheckMultipleAnswers_ReturnsAnswerResultForEachEntry()
        {
            // Arrange  
            using var ctx = Fixture.CreateDbContext();
            ctx.OpenTriviaResults.AddRange(
                new OpenTriviaResultDTO
                {
                    Id = 1,
                    Question = "Q1",
                    Type = "test",
                    Difficulty = "test",
                    Category = "test",
                    CorrectAnswer = "A",
                    IncorrectAnswers = [ "B", "C", "D" ]
                },
                new OpenTriviaResultDTO
                {
                    Id = 2,
                    Question = "Q2",
                    Type = "test",
                    Difficulty = "test",
                    Category = "test",
                    CorrectAnswer = "True",
                    IncorrectAnswers = [ "False" ]
                }
            );
            ctx.SaveChanges();

            var service = CreateService(ctx);

            var input = new Dictionary<int, string>
            {
                [1] = "A",
                [2] = "False"
            };

            // Act
            var results = service.CheckMultipleAnswers(input);

            // Assert
            results.Should().HaveCount(2);

            results.Single(r => r.Id == 1).IsCorrectAnswer.Should().Be("True");
            results.Single(r => r.Id == 2).IsCorrectAnswer.Should().Be("False");

            ctx.Database.EnsureDeleted();
        }

        private OpenTriviaApiService CreateService(OpenTriviaContext context)
        {
            var loggerMock = new Mock<ILogger<OpenTriviaApiService>>();
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://opentdb.com/")
            };
            return new OpenTriviaApiService(loggerMock.Object, context, httpClient);
        }
    }
}
