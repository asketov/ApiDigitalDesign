using ApiDigitalDesign.Models.AuthModels;
using ApiDigitalDesign.Models.UserModels;
using ApiDigitalDesign.Services;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public AuthController(AuthService authService, IMapper mapper, UserService userService)
        {
            _authService = authService;
            _mapper = mapper;
            _userService = userService;
        }
        [HttpPost]
        public async Task<ActionResult> SignIn(SignInRequest request)
        {
            try
            {
                var model = _mapper.Map<SignInModel>(request);
                var response = await _authService.GetTokensAsync(model);
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
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel request)
        {
            try
            {
                await _userService.CreateUserAsync(request);
                var model = _mapper.Map<SignInModel>(request);
                var tokens = await _authService.GetTokensAsync(model);
                return Ok(tokens);
            }
            catch (UserAlreadyExistException)
            {
                return new JsonResult(new { message = "User is already exist" })
                    { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch
            {
                return new JsonResult(new { message = "Server can't process the request" })
                    { StatusCode = StatusCodes.Status503ServiceUnavailable };
            }
        }
    }
}
