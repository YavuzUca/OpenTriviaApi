using Trivia.Models;
using Trivia.Models.DTO;

namespace Trivia.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetApiResult();
        Task SaveUsersAsync();
        List<UserDto> GetUsersFromDb();
        List<UserDto> SortUsers(List<UserDto> users);
    }

    public class UserService : OpenTriviaBaseService, IUserService
    {
        private readonly ILogger<UserService> Logger;
        private readonly OpenTriviaContext DbContext;

        public UserService(
            ILogger<UserService> logger,
            OpenTriviaContext context,
            HttpClient httpClient
            ) : base(httpClient)
        {
            Logger = logger;
            DbContext = context;
        }

        public async Task<List<UserDto>> GetApiResult()
        {
            string endpoint = httpClient.BaseAddress + "users";

            return await GenericOpenTriviaGetJSON<List<UserDto>>(endpoint);
        }

        public async Task SaveUsersAsync()
        {
            var apiResponse = await this.GetApiResult();

            if (apiResponse != null)
            {
                DbContext.Users.RemoveRange(DbContext.Users);
                DbContext.Users.AddRange(apiResponse);
                await DbContext.SaveChangesAsync();
            }
        }

        public List<UserDto> GetUsersFromDb()
        {
            return [.. DbContext.Users];
        }

        public List<UserDto> SortUsers(List<UserDto> users)
        {
            return users.OrderBy(x => x.Name).ToList();
        }
    }
}
