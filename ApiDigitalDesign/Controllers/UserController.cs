using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.ModelsDTO.UserModels;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;


namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Create user in system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Post /createuser body: { "name": "string", "email": "user@example.com", "password": "string",
        /// "confirmPassword": "string", "birthDate": "2022-10-29T19:11:31.165Z" }
        /// </remarks>
        /// <returns>Returns Created Resource</returns>
        /// <response code="400">Fields is not valid</response>
        /// <response code="200">Success, in response userId</response>
        /// <response code="503">If the user is not created</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateUser(CreateUserDTO dto)
        {
            try
            {
                var userId = await _userService.CreateUserAsync(dto);
                return new JsonResult(new { message = $"Server created new User with id:{userId}" })
                    { StatusCode = StatusCodes.Status201Created };
            }
            catch
            {
                return new JsonResult(new { message = "Server can't process the request" }) 
                    { StatusCode = StatusCodes.Status503ServiceUnavailable };
            }
        }
    }
}
