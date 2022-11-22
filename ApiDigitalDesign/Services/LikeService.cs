using ApiDigitalDesign.Models.LikeModels;
using AutoMapper;
using Common.Exceptions.Like;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiDigitalDesign.Services
{
    public class LikeService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _db;
        public LikeService(IMapper mapper, DataContext db)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task AddLikeToComment(LikeCommentModel request)
        {
            if (await GetCommentLike(request) != null) throw new LikeAlreadyExistException();
            var model = _mapper.Map<CommentLike>(request);
            _db.CommentLikes.Add(model);
            await _db.SaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="LikeAlreadyExistException"></exception>
        public async Task AddLikeToPost(LikePostModel request)
        {
            if (await GetPostLike(request) != null) throw new LikeAlreadyExistException();
            var model = _mapper.Map<PostLike>(request);
            _db.PostLikes.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task<Like?> GetPostLike(LikePostModel model)
        {
            var like = await _db.PostLikes
                .FirstOrDefaultAsync(f => f.AuthorId == model.AuthorId && f.PostId == model.PostId);
            return like;
        }
        public async Task<Like?> GetCommentLike(LikeCommentModel model)
        {
            var like = await _db.CommentLikes
                .FirstOrDefaultAsync(f => f.AuthorId == model.AuthorId && f.CommentId == model.CommentId);
            return like;
        }
    }
}
