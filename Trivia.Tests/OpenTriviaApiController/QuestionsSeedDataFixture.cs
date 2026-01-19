using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Trivia.Models;
using Trivia.Models.DTO;

namespace Trivia.Tests.OpenTriviaApiController
{
    public class QuestionsSeedDataFixture
    {

        public OpenTriviaContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<OpenTriviaContext>()
                .UseInMemoryDatabase("Questions")
                .Options;

            var DbContext = new OpenTriviaContext(options);

            // Seed data
            // DbContext.SaveChanges();

            return DbContext;
        }
    }
}
