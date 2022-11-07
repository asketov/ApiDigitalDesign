using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Services;
using Common.Exceptions.Attach;
using Common.Exceptions.User;
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
        /// <summary>
        /// Create files in tempDirectory
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Post /uploadfiles body: { files... }
        /// </remarks>
        /// <returns>Returns locations created resources</returns>
        /// <response code="400">Files already exist</response>
        /// <response code="200">Success, in response all info about files</response>
        /// <response code="503">TempDirectory not exist</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [HttpPost]
        public async Task<ActionResult<List<MetadataModel>>> UploadFiles([FromForm] List<IFormFile> files)
        {
            try
            {
               var tempFiles = await _attachService.UploadFiles(files);
               return Ok(tempFiles);
            }
            catch (FileAlreadyExistException)
            {
                return new JsonResult(new {message = "One of Files in tempDirectory is already exist"})
                    {StatusCode = StatusCodes.Status400BadRequest};
            }
            catch (DirectoryNotExistException)
            {
                return new JsonResult(new { message = "Temp Folder is not exist" })
                    { StatusCode = StatusCodes.Status503ServiceUnavailable };
            }
        }
    }
}
