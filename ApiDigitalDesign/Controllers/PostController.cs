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
using ApiDigitalDesign.Models.LikeModels;
using AutoMapper;
using Common.Exceptions.Like;

namespace ApiDigitalDesign.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class PostController : BaseController
    {
        private readonly PostService _postService;
        private readonly LikeService _likeService;
        private readonly IMapper _mapper;

        public PostController(PostService postService, LikeService likeService, LinkGeneratorService _links, IMapper mapper)
        {
            _postService = postService;
            _likeService = likeService;
            _links.LinkContentGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetPostAttach), 
            new {
                postAttachId = x.Id
            });
            _links.LinkAvatarGenerator = x =>
                Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
                {
                    userId = x.Id
                });
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreatePost(CreatePostRequest request)
        {
            try
            {
                request.AuthorId = UserId;
                var model = _mapper.Map<CreatePostModel>(request);
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
        public async Task<ActionResult> GetPostsForTape(int skip = 0, int take = 10)
        {
            var posts = await _postService.GetPostsSubscriptions(UserId, skip, take);
            return Ok(posts);
            
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetUserPosts(int skip = 0, int take = 10)
        {
            var posts = await _postService.GetUserPosts(UserId, skip, take);
            return Ok(posts);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateComment(CreateCommentRequest request)
        {
            try
            {
                request.AuthorId = UserId;
                var model = _mapper.Map<CreateCommentModel>(request);
                await _postService.CreateCommentAsync(model);
                return new JsonResult(new {message = $"Server created new Comment"})
                    {StatusCode = StatusCodes.Status200OK};
            }
            catch (PostNotFoundException ex)
            {
                return new JsonResult(new {message = ex.Message})
                    {StatusCode = StatusCodes.Status404NotFound};
            }
            //catch
            //{
            //    return new JsonResult(new { message = "Server can't process the request" })
            //        { StatusCode = StatusCodes.Status503ServiceUnavailable };
            //}
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<CommentModel>>> GetPostComments(Guid postId, int skip = 0, int take = 10)
        {
            return await _postService.GetPostComments(skip, take, postId);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddLikeToComment(LikeCommentModel request)
        {
            try
            {
                request.AuthorId = UserId;
                await _likeService.AddLikeToComment(request);
                return Ok();
            }
            catch (LikeAlreadyExistException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddLikeToPost(LikePostModel request)
        {
            try
            {
                request.AuthorId = UserId;
                await _likeService.AddLikeToPost(request);
                return Ok();
            }
            catch (LikeAlreadyExistException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> DeleteLikeFromComment(LikeCommentModel request)
        {
            try
            {
                request.AuthorId = UserId;
                await _likeService.DeleteCommentLike(request);
                return Ok();
            }
            catch(LikeNotFoundException)
            {
                return BadRequest("Лайк не найден");
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> DeleteLikeFromPost(LikePostModel request)
        {
            try
            {
                request.AuthorId = UserId;
                await _likeService.DeletePostLike(request);
                return Ok();
            }
            catch (LikeNotFoundException)
            {
                return BadRequest("Лайк не найден");
            }
        }

    }
}
