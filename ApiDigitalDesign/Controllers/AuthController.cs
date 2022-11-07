using ApiDigitalDesign.Models.AuthModels;
using ApiDigitalDesign.Services;
using Common.Exceptions.Auth;
using Common.Exceptions.User;
using Microsoft.AspNetCore.Mvc;

namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// Sign in system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Post /SignIn body: { "email": "user@example.com", "password": "string" }
        /// </remarks>
        /// <returns>Returns Access + Refresh tokens</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Fields is not valid</response>
        /// <response code="404">If the user is not exist or data is incorrect</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<ActionResult> SignIn(SignInModel dto)
        {
            try
            {
                var response = await _authService.GetTokensAsync(dto);
                return Ok(response);
            }
            catch(UserNotFoundException)
            {
                return new JsonResult(new { message = "the server did not find such " +
                                                      "a user or the entered data was incorrect" })
                    { StatusCode = StatusCodes.Status404NotFound };
            }
        }
        /// <summary>
        /// Sign in system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Post /login body: { "email": "user@example.com", "password": "string" }
        /// </remarks>
        /// <returns>Returns Access + Refresh tokens</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Fields or token is not valid</response>
        /// <response code="404">If the user is not exist or data is incorrect</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<ActionResult> RefreshToken(string refreshToken)
        {
            try
            {
                var tokenModel = await _authService.GetTokensByRefreshAsync(refreshToken);
                return Ok(tokenModel);
            }
            catch (UserNotFoundException)
            {
                return new JsonResult(new {message = "the server did not find such a user"}) 
                    {StatusCode = StatusCodes.Status404NotFound};
            }
            catch (InvalidTokenException)
            {
                return new JsonResult(new {message = "token is invalid"})
                    { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
    }
}
