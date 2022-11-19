using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiDigitalDesign.Models.PostModels;
using ApiDigitalDesign.Services;
using Common.Exceptions.Attach;
using Common.Exceptions.Posts;
using Common.Exceptions.User;
using Common.Generics;
using DAL.Entities;
using System.ComponentModel.Design;

namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class PostController : BaseController
    {
        private readonly PostService _postService;
        private readonly LikeService _likeService;


        public PostController(PostService postService, LikeService likeService, LinkGeneratorService _links)
        {
            _postService = postService;
            _likeService = likeService;
            _links.LinkContentGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetPostAttach), 
            new {
                postAttachId = x.Id
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
            catch
            {
                return new JsonResult(new { message = "Server can't process the request" })
                { StatusCode = StatusCodes.Status503ServiceUnavailable };
            }
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
        public async Task<ActionResult> CreateComment(CreateCommentRequest model)
        {
            try
            {
                model.AuthorId = UserId;
                var commendId = await _postService.CreateCommentAsync(model);
                return new JsonResult(new {message = $"Server created new Comment with id:{commendId}"})
                    {StatusCode = StatusCodes.Status200OK};
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

        [HttpPost]
        [Authorize]
        public async Task AddLikeToComment(Guid commentId)
        {
           await _likeService.AddLikeToComment(commentId, UserId);
        }

        [HttpPost]
        [Authorize]
        public async Task AddLikeToPost(Guid postId)
        {
            await _likeService.AddLikeToPost(postId, UserId);

        }
    }
}
