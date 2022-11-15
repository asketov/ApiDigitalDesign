using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Models.PostModels;
using ApiDigitalDesign.Services;
using Common.Exceptions.Attach;
using Common.Exceptions.Posts;
using Common.Exceptions.User;
using Common.Statics;

namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : BaseController
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreatePost(CreatePostModel model)
        {
            if (UserId != default)
            {
                try
                {
                    var postId = await _postService.CreatePostAsync(model, UserId);
                    return new JsonResult(new {message = $"Server created new Post with id:{postId}"})
                        {StatusCode = StatusCodes.Status200OK};
                }
                catch (UserNotFoundException ex)
                {
                    return new JsonResult(new {message = ex.Message })
                        {StatusCode = StatusCodes.Status401Unauthorized};
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
            else
                return new JsonResult(new { message = "Unauthorized" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
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
            if (UserId != default)
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
            else
                return new JsonResult(new { message = "Unauthorized" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
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
