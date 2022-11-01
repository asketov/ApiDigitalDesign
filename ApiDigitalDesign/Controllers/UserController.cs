using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.UserModels;
using ApiDigitalDesign.Services;
using Common.Exceptions.General;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
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
        /// <response code="400">Fields is not valid or user already exist</response>
        /// <response code="201">Success, in response userId</response>
        /// <response code="503">If the user is not created</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateUser(CreateUserModel dto)
        {
            try
            {
                var userId = await _userService.CreateUserAsync(dto);
                return new JsonResult(new {message = $"Server created new User with id:{userId}"})
                    {StatusCode = StatusCodes.Status201Created};
            }
            catch (AlreadyExistException)
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
        /// <summary>
        /// Get current user
        /// [Not completed]
        /// While use this method for integration testing auth
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetCurrentUser()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (Guid.TryParse(userIdString, out var userId))
            {

                var user = await _userService.GetUserAsync(userId);
                return Ok(user);
            }
            else
                return new JsonResult(new { message = "you are not authorized" })
                    { StatusCode = StatusCodes.Status401Unauthorized };

        }
    }
}
