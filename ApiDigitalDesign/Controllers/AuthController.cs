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
        public AuthController(AuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
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
    }
}
