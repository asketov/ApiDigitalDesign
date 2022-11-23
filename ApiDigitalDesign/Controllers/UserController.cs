using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Models.AuthModels;
using ApiDigitalDesign.Models.UserModels;
using ApiDigitalDesign.Services;
using AutoMapper;
using Common.Exceptions.Attach;
using Common.Exceptions.User;
using Common.Generics;
using Common.Statics;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        public UserController(UserService userService, LinkGeneratorService links, AuthService authService, IMapper mapper)
        {
            _userService = userService;
            links.LinkAvatarGenerator = x =>
                Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
                {
                    userId = x.Id
                });
            _authService = authService;
            _mapper = mapper;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddAvatarToUser(MetadataModel model)
        {
            try
            {
                await _userService.AddAvatarToUserAsync(UserId, model);
                return new JsonResult(new {message = $"Server created new Avatar for current user"})
                    { StatusCode = StatusCodes.Status201Created };
            }
            catch (FileNotExistException)
            {
                return new JsonResult(new { message = "File not exist" })
                    { StatusCode = StatusCodes.Status400BadRequest };
            }
                
        }

        [HttpGet]
        public async Task<ActionResult> DownloadAvatar(Guid userId)
        {
            try
            {
                var attach = await _userService.GetUserAvatarAsync(userId);
                HttpContext.Response.ContentType = attach.MimeType;
                FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(attach.FilePath), attach.MimeType)
                {
                    FileDownloadName = attach.Name
                };
                return result;
            }
            catch (AvatarNotFoundException ex)
            {
                return new JsonResult(new { message = ex.Message })
                    { StatusCode = StatusCodes.Status404NotFound };
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetCurrentUser()
        {
            var model = await _userService.GetUserModelAsync(UserId);
            return Ok(model);
        }

        //[HttpGet]
        //public async Task<ActionResult<List<UserAvatarModel>>> GetAllUsers()
        //{
        //    var users = await _userService.GetUsersAsync();
        //    return Ok(users);
        //}

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangeVisibilityAccount()
        {
            try
            {
                await _userService.ChangeVisibilityAccount(UserId);
                return new JsonResult(new { message = "Visibility successfully changed" })
                    { StatusCode = StatusCodes.Status200OK };
            }
            catch
            {
                return new JsonResult(new { message = "Service in unavailable" })
                    { StatusCode = StatusCodes.Status503ServiceUnavailable };
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> DeleteAccount()
        {
           await _userService.DeleteAccount(UserId);
           return new JsonResult(new { message = "Account successfully deleted" })
               { StatusCode = StatusCodes.Status200OK };
        }
    }
}
