using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Services;
using Common.Exceptions.Attach;
using Common.Exceptions.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class AttachController : BaseController
    {
        private readonly AttachService _attachService;
        private readonly PostService _postService;
        private readonly UserService _userService;
        public AttachController(AttachService attachService, UserService userService, PostService postService)
        {
            _attachService = attachService;
            _userService = userService;
            _postService = postService;
        }
        
        [HttpPost]
        public async Task<ActionResult<List<MetadataModel>>> UploadFiles([FromForm] List<IFormFile> files)
        {
            try
            {
               var tempFiles = await _attachService.UploadFiles(files);
               return Ok(tempFiles);
            }
            catch (FileAlreadyExistException ex)
            {
                return new JsonResult(new {message = ex.Message})
                    {StatusCode = StatusCodes.Status400BadRequest};
            }
            catch (DirectoryNotExistException ex)
            {
                return new JsonResult(new { message = ex.Message })
                    { StatusCode = StatusCodes.Status503ServiceUnavailable };
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAttach(Guid attachId, bool download = false)
        {
            try
            {
                var attachModel = await _attachService.GetAttachAsync(attachId);
                return RenderAttach(attachModel, download);
            }
            catch (AttachNotFoundException ex)
            {
                return new JsonResult(new { message = ex.Message })
                    { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
        [HttpGet]
        [Route("{postAttachId}")]
        public async Task<ActionResult> GetPostAttach(Guid postAttachId, bool download = false)
        {
            try
            {
                var attach = await _postService.GetPostAttachAsync(postAttachId);
                return RenderAttach(attach, download);
            }
            catch (AttachNotFoundException ex)
            {
                return new JsonResult(new { message = ex.Message })
                    { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch
            {
                return new JsonResult(new { message = "Server can't process the request" })
                    { StatusCode = StatusCodes.Status503ServiceUnavailable };
            }
        }
        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult> GetUserAvatar(Guid userId, bool download = false)
        {
            try
            {
                var attach = await _userService.GetUserAvatarAsync(userId);
                return RenderAttach(attach, download);
            }
            catch (AvatarNotFoundException)
            {
                return new JsonResult(new { message = "Avatar not found" })
                    { StatusCode = StatusCodes.Status404NotFound };
            }
        }
        private FileStreamResult RenderAttach(AttachModel attach, bool download)
        {
            var fs = new FileStream(attach.FilePath, FileMode.Open);
            var ext = Path.GetExtension(attach.Name);
            if (download)
                return File(fs, attach.MimeType, $"{ attach.Id }{ext}");
            return File(fs, attach.MimeType);

        }
    }
}
