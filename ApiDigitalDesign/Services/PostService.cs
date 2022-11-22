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
        /// <param name="model"></param>
        /// <returns>Guid id created post.</returns>
        /// <exception cref="FileNotExistException">one of the files don't exist</exception>
        public async Task<Guid> CreatePostAsync(CreatePostModel model)
        {
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
        
        public async Task<List<PostModel>> GetUserPosts(Guid userId, int skip, int take)
        {
            var posts = await _db.Posts.Where(u=>u.AuthorId == userId)
                .Include(x => x.PostAttaches).Include(f=>f.Comments).Include(f => f.Likes)
                .AsNoTracking().OrderByDescending(x => x.Created).Skip(skip).Take(take)
                .Select(x => _mapper.Map<PostModel>(x))
                .ToListAsync();
            return posts;
        }
        public async Task<List<PostModel>> GetPostsSubscriptions(Guid userId, int skip, int take)
        {
            var subscribes = await _db.Subscribes
                .Where(u => u.SubscriberId == userId).Include(x => x.Recipient)
                .ThenInclude(u => u.Posts)!.ThenInclude(u => u.PostAttaches)
                .Include(x => x.Recipient).ThenInclude(f=>f.Posts)!.ThenInclude(f => f.Comments)
                .Include(x => x.Recipient).ThenInclude(f => f.Posts)!.ThenInclude(f => f.Likes)
                .AsNoTracking().ToListAsync();
            //var subscribes1 = _db.Subscribes.Where(u => u.SubscriberId == userId).Select(e => new
            //{
            //    PostAttaches = e.Recipient.Posts!.Select(f => f.PostAttaches),
            //    CommentsCount = e.Recipient.Posts!.Select(f => f.Comments).Count(),
            //    LikesCount = e.Recipient.Posts!.Select(f => f.Likes).Count()
            //}).AsNoTracking().AsQueryable();
            var posts = subscribes.SelectMany(x => x.Recipient.Posts!.Skip(skip).Take(take))
                .Select(x => _mapper.Map<PostModel>(x)).ToList();
            return posts;
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
        /// <param name="model"></param>
        /// <exception cref="PostNotFoundException"></exception>
        public async Task CreateCommentAsync(CreateCommentModel model)
        {
            var post = await GetFullPostAsync(model.PostId);
            var comm = _mapper.Map<Comment>(model);
            post.Comments!.Add(comm);
            await _db.SaveChangesAsync();
        }
        /// <summary>
        /// Get full Post with all relations
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        /// <exception cref="PostNotFoundException"></exception>
        public async Task<Post> GetFullPostAsync(Guid postId)
        {
            var post = await _db.Posts.Include(u=>u.Comments).Include(k=>k.Author).Include(k=>k.PostAttaches).Include(f=>f.Likes)
                .FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null) throw new PostNotFoundException("Such post not exist.");
            return post;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<List<CommentModel>> GetPostComments(int skip, int take, Guid postId)
        {
            var comments = await _db.Comments.Where(comm=>comm.PostId == postId)
                .OrderByDescending(u=>u.Created).Skip(skip).Take(take).AsNoTracking()
                .ProjectTo<CommentModel>(_mapper.ConfigurationProvider).ToListAsync();
            return comments;
        }

    }
}
