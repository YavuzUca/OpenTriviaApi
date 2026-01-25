using Microsoft.AspNetCore.Mvc;
using Trivia.Models;
using Trivia.Models.DTO;
using Trivia.Services;

namespace Trivia.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet("users")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            await _userService.SaveUsersAsync();

            var users = _userService.GetUsersFromDb();
            var sortedUsers = _userService.SortUsers(users);
            return Ok(sortedUsers);
        }
    }
}
