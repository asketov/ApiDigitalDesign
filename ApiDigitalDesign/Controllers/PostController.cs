using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiDigitalDesign.Models.PostModels;
using ApiDigitalDesign.Services;
using Common.Exceptions.Attach;
using Common.Exceptions.Posts;
using Common.Exceptions.User;
using Common.Generics;
using DAL.Entities;

namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class PostController : BaseController
    {
        private readonly PostService _postService;

        public PostController(PostService postService, LinkGeneratorService _links)
        {
            _postService = postService;
            _links.LinkContentGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetPostAttach), new
            {
                postAttachId = x.Post.Id
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreatePost(CreatePostRequest model)
        {
            try
            {
                model.AuthorId = UserId;
                var postId = await _postService.CreatePostAsync(model);
                return new JsonResult(new {message = $"Server created new Post with id:{postId}"})
                    {StatusCode = StatusCodes.Status200OK};
            }
            catch (FileNotExistException ex)
            {
                return new JsonResult(new { message = ex.Message })
                    {StatusCode = StatusCodes.Status400BadRequest};
            }
            //catch
            //{
            //    return new JsonResult(new { message = "Server can't process the request" }) 
            //        { StatusCode = StatusCodes.Status503ServiceUnavailable };
            //}
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PostModel>> GetPost(Guid postId)
        {
            try
            {
                var postModel = await _postService.GetPostModel(postId);
                return Ok(postModel);
            }
            catch (PostNotFoundException ex)
            {
                return new JsonResult(new { message = ex.Message })
                    { StatusCode = StatusCodes.Status404NotFound };
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateComment(CreateCommentModel model)
        {
            try
            {
                var commendId = await _postService.CreateCommentAsync(model, UserId);
                return new JsonResult(new {message = $"Server created new Comment with id:{commendId}"})
                    {StatusCode = StatusCodes.Status200OK};
            }
            catch (UserNotFoundException ex)
            {
                return new JsonResult(new {message = ex.Message})
                    {StatusCode = StatusCodes.Status401Unauthorized};
            }
            catch (PostNotFoundException ex)
            {
                return new JsonResult(new {message = ex.Message})
                    {StatusCode = StatusCodes.Status404NotFound};
            }
            catch
            {
                return new JsonResult(new { message = "Server can't process the request" })
                    { StatusCode = StatusCodes.Status503ServiceUnavailable };
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<CommentModel>>> GetPostComments(Guid postId)
        {
            try
            {
                return await _postService.GetAllPostComments(postId);
            }
            catch (CommentNotFoundException ex)
            {
                return new JsonResult(new { message = ex.Message })
                    { StatusCode = StatusCodes.Status404NotFound };
            }
        }
    }
}
