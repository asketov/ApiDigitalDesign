using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Models.UserModels;
using ApiDigitalDesign.Services;
using Common.Exceptions.Attach;
using Common.Exceptions.User;
using Common.Statics;
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
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddAvatarToUser(MetadataModel model)
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == Auth.UserClaim)?.Value;
            if (Guid.TryParse(userIdString, out var userId))
            {
                try
                {
                    Guid avatarId = await _userService.AddAvatarToUser(userId, model);
                    return new JsonResult(new {message = $"Server created new Avatar with id:{avatarId}"})
                        { StatusCode = StatusCodes.Status201Created };
                }
                catch (UserNotFoundException)
                {
                    return new JsonResult(new {message = "User not found"})
                        {StatusCode = StatusCodes.Status404NotFound};
                }
                catch (FileNotExistException)
                {
                    return new JsonResult(new { message = "File not exist in tempDirectory" })
                        { StatusCode = StatusCodes.Status400BadRequest };
                }
            }
            else
                return new JsonResult(new { message = "Unauthorized" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
        }

        [HttpGet]
        public async Task<ActionResult> GetUserAvatar(Guid userId)
        {
            try
            {
                var attach = await _userService.GetUserAvatar(userId);
                return File(System.IO.File.ReadAllBytes(attach.FilePath), attach.MimeType);
            }
            catch(UserNotFoundException ex)
            {
                return new JsonResult(new { message = "User not found" })
                    { StatusCode = StatusCodes.Status404NotFound };
            }
        }

        [HttpGet]
        public async Task<ActionResult> DownloadAvatar(Guid userId)
        {
            try
            {
                var attach = await _userService.GetUserAvatar(userId);
                HttpContext.Response.ContentType = attach.MimeType;
                FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(attach.FilePath), attach.MimeType)
                {
                    FileDownloadName = attach.Name
                };
                return result;
            }
            catch (UserNotFoundException ex)
            {
                return new JsonResult(new { message = "User not found" })
                    { StatusCode = StatusCodes.Status404NotFound };
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel dto)
        {
            try
            {
                var userId = await _userService.CreateUserAsync(dto);
                return new JsonResult(new {message = $"Server created new User with id:{userId}"})
                    {StatusCode = StatusCodes.Status201Created};
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
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetCurrentUser()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == Auth.UserClaim)?.Value;
            if (Guid.TryParse(userIdString, out var userId))
            {
                var user = await _userService.GetUserModel(userId);
                return Ok(user);
            }
            else
                return new JsonResult(new { message = "you are not authorized" })
                    { StatusCode = StatusCodes.Status401Unauthorized };

        }
    }
}
