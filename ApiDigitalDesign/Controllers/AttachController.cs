using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Services;
using Common.Exceptions.Attach;
using Common.Exceptions.User;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttachController : ControllerBase
    {
        private readonly AttachService _attachService;
        public AttachController(AttachService attachService)
        {
            _attachService = attachService;
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
        public async Task<ActionResult> GetAttach(Guid attachId)
        {
            try
            {
                var attachModel = await _attachService.GetAttachAsync(attachId);
                return File(System.IO.File.ReadAllBytes(attachModel.FilePath), attachModel.MimeType);
            }
            catch (AttachNotFoundException ex)
            {
                return new JsonResult(new { message = ex.Message })
                    { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
    }
}
