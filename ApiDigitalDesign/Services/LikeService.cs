using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using DAL.Entities;

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

        public async Task AddLikeToComment(Guid CommentId, Guid AuthorId)
        {
            var likeModel = new CommentLike()
            {
                Created = DateTime.UtcNow, AuthorId = AuthorId, CommentId = CommentId
            };
            _db.CommentLikes.Add(likeModel);
            await _db.SaveChangesAsync();
        }

        public async Task AddLikeToPost(Guid PostId, Guid AuthorId)
        {
            var likeModel = new PostLike()
            {
                Created = DateTime.UtcNow,
                AuthorId = AuthorId, PostId = PostId
            };
            _db.PostLikes.Add(likeModel);
            await _db.SaveChangesAsync();
        }
    }
}
