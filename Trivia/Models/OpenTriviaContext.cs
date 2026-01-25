using Microsoft.EntityFrameworkCore;
using Trivia.Models.DTO;

namespace Trivia.Models
{
    public class OpenTriviaContext : DbContext
    {
        public OpenTriviaContext(DbContextOptions<OpenTriviaContext> options)
        : base(options)
        {
        }

        public DbSet<OpenTriviaResultDTO> OpenTriviaResults { get; set; } = null!;
        public DbSet<UserDto> Users { get; set; } = null!;
    }
}
