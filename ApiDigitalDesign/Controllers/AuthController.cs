using ApiDigitalDesign.Models.AuthModels;
using ApiDigitalDesign.Services;
using Common.Exceptions.Auth;
using Common.Exceptions.User;
using Microsoft.AspNetCore.Mvc;

namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<ActionResult> SignIn(SignInModel dto)
        {
            try
            {
                var response = await _authService.GetTokensAsync(dto);
                return Ok(response);
            }
            catch(UserNotFoundException ex)
            {
                return new JsonResult(new { message = ex.Message })
                    { StatusCode = StatusCodes.Status404NotFound };
            }
        }
        [HttpPost]
        public async Task<ActionResult> RefreshToken(string refreshToken)
        {
            try
            {
                var tokenModel = await _authService.GetTokensByRefreshAsync(refreshToken);
                return Ok(tokenModel);
            }
            catch (UserNotFoundException ex)
            {
                return new JsonResult(new {message = ex.Message}) 
                    {StatusCode = StatusCodes.Status404NotFound};
            }
            catch (InvalidTokenException ex)
            {
                return new JsonResult(new {message = ex.Message})
                    { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
    }
}
