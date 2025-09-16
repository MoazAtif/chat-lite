using Microsoft.EntityFrameworkCore;

namespace tring_to_be_better.Model
{
    public class AppDbcontext :DbContext
    {
        public AppDbcontext(DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>().HasKey(l => new { l.PostId, l.UserId });
            
            
            
        }
        public DbSet<User> users { get; set; }
        public DbSet<Post> posts { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Like> like { get; set; }
    }
}
