using ApiDigitalDesign.Models.AttachModels;
using ApiDigitalDesign.Models.PostModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions.Attach;
using Common.Exceptions.Posts;
using Common.Exceptions.User;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace ApiDigitalDesign.Services
{
    public class PostService
    {
        private readonly DataContext _db;
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        public PostService(DataContext db, UserService userService, IMapper mapper)
        {
            _db = db;
            _userService = userService;
            _mapper = mapper;
        }
        /// <summary>
        /// Create Post with attaches.(need refactor)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Guid id created post.</returns>
        /// <exception cref="FileNotExistException">one of the files don't exist</exception>
        public async Task<Guid> CreatePostAsync(CreatePostRequest request)
        {
            var model = _mapper.Map<CreatePostModel>(request);
            model.Attaches.ForEach(q =>
            {
                q.FilePath = AttachService.CopyTempFileToAttaches(q.TempId);
                q.AuthorId = model.AuthorId;
            });
            var post = _mapper.Map<Post>(model);
            var t = _db.Posts.Add(post);
            await _db.SaveChangesAsync();
            return t.Entity.Id;
        }
        /// <summary>
        /// get postModel by guid postId (need refactor)
        /// </summary>
        /// <param name="postId"></param>
        /// <returns><see cref="PostModel"/></returns>
        /// <exception cref="PostNotFoundException"></exception>
        public async Task<PostModel> GetPostModel(Guid postId)
        {
            var post = await _db.Posts.Include(x => x.Author).Include(x=>x.PostAttaches).AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == postId);
            if (post != null)
            {
                var getPostModel = _mapper.Map<PostModel>(post);
                return getPostModel;
            }
            throw new PostNotFoundException("post with such id not found");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postAttachId"></param>
        /// <returns></returns>
        /// <exception cref="AttachNotFoundException"></exception>
        public async Task<AttachModel> GetPostAttachAsync(Guid postAttachId)
        {
            var postAttach = await _db.PostAttaches.FirstOrDefaultAsync(u => u.Id == postAttachId);
            if (postAttach == null) throw new AttachNotFoundException("attach not found");
            var attach = _mapper.Map<AttachModel>(postAttach);
            return attach;
        }
        /// <summary>
        /// Returns id created comment
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="PostNotFoundException"></exception>
        public async Task<Guid> CreateCommentAsync(CreateCommentRequest request)
        {
            var model = _mapper.Map<CreateCommentModel>(request);
            var post = await GetClearPostAsync(model.PostId);
            var comm = _mapper.Map<Comment>(model);
            var t = _db.Comments.Add(comm);
            await _db.SaveChangesAsync();
            return t.Entity.Id;
        }
        /// <summary>
        /// Get Clear Post without relations
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        /// <exception cref="PostNotFoundException"></exception>
        public async Task<Post> GetClearPostAsync(Guid postId)
        {
            var post = await _db.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null) throw new PostNotFoundException("Such post not exist.");
            return post;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        /// <exception cref="CommentNotFoundException"></exception>
        public async Task<List<CommentModel>> GetAllPostComments(Guid postId)
        {
            var comments = await _db.Comments.Where(comm=>comm.PostId==postId).AsNoTracking()
                .ProjectTo<CommentModel>(_mapper.ConfigurationProvider).ToListAsync();
            if (!comments.Any())
                throw new CommentNotFoundException("In this post not exist anyone comment or postId invalid");
            return comments;
        }

    }
}
