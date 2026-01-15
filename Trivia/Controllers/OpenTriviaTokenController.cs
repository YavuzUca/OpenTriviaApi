using Microsoft.AspNetCore.Mvc;
using Trivia.Models;
using Trivia.Models.DTO;
using Trivia.Services;

namespace Trivia.Controllers
{
    [ApiController]
    [Route("api/")]
    public class OpenTriviaTokenController(IOpenTriviaTokenService tokenService) : ControllerBase
    {
        private readonly IOpenTriviaTokenService _tokenService = tokenService;

        [HttpGet("getToken")]
        public async Task<ActionResult<OpenTriviaTokenDTO>> GetToken()
        {
            var response = await _tokenService.RequestSessionToken();
            var code = (ResponseCode)response.ResponseCode;

            return code switch
            {
                ResponseCode.Success => Ok(response),
                ResponseCode.NoResults => NotFound(),
                ResponseCode.Invalid => BadRequest("Invalid parameters."),
                ResponseCode.TokenNotFound => BadRequest("The token used has not been found."),
                ResponseCode.TokenEmpty => BadRequest("The token used has not been found."),
                ResponseCode.RateLimit => BadRequest("Rate limit has been reached."),
                _ => StatusCode(502)
            };
        }

        [HttpGet("refreshToken")]
        public async Task<ActionResult<OpenTriviaTokenDTO>> RefreshToken([FromQuery] string token)
        {
            var isValidToken = _tokenService.IsValidToken(token);

            if (token == null || !isValidToken)
                return BadRequest("Invalid parameters.");

            var response = await _tokenService.ResetSessionToken(token);
            var code = (ResponseCode)response.ResponseCode;

            return code switch
            {
                ResponseCode.Success => Ok(response),
                ResponseCode.NoResults => NotFound(),
                ResponseCode.Invalid => BadRequest("Invalid parameters."),
                ResponseCode.TokenNotFound => BadRequest("The token used has not been found."),
                ResponseCode.TokenEmpty => BadRequest("The token used has not been found."),
                ResponseCode.RateLimit => BadRequest("Rate limit has been reached."),
                _ => StatusCode(502)
            };
        }
    }
}
