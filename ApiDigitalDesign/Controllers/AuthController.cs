using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ModelsDTO.AuthModels;
using BLL.Services;
using Common.Exceptions.General;
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
        /// Post /login body: { "email": "user@example.com", "password": "string" }
        /// </remarks>
        /// <returns>Returns Access + Refresh tokens</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Fields is not valid</response>
        /// <response code="404">If the user is not exist or data is incorrect</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult> Login(SignInDTO dto)
        {
            try
            {
                var response = await _authService.LoginAsync(dto);
                return Ok(response);
            }
            catch(NotFoundException)
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
        /// <response code="400">Fields is not valid</response>
        /// <response code="404">If the user is not exist or data is incorrect</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public ActionResult RefreshToken(string refreshToken)
        {
            try
            {
                var tokenModel = _authService.RefreshToken(refreshToken);
                return Ok(tokenModel);
            }
            catch (NotFoundException)
            {
                return new JsonResult(new
                    {
                        message = "the server did not find such a user"
                    })
                    { StatusCode = StatusCodes.Status404NotFound };
            }
        }

    }
}
