using DAL.Entities;
using DAL.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(b => b.MigrationsAssembly("ApiDigitalDesign"));
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserSessionConfiguration());
            builder.ApplyConfiguration(new AvatarConfiguration());
            builder.ApplyConfiguration(new PostAttachConfiguration());
            builder.ApplyConfiguration(new CommentLikeConfiguration());
            builder.ApplyConfiguration(new PostLikeConfiguration());
            builder.ApplyConfiguration(new SubscribeConfiguration());
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserSession> UserSessions => Set<UserSession>();
        public DbSet<Avatar> Avatars => Set<Avatar>();
        public DbSet<Attach> Attaches => Set<Attach>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<PostAttach> PostAttaches => Set<PostAttach>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<PostLike> PostLikes => Set<PostLike>();
        public DbSet<CommentLike> CommentLikes => Set<CommentLike>();
        public DbSet<Subscribe> Subscribes => Set<Subscribe>();
    }
}
