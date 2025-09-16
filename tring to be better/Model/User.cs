namespace tring_to_be_better.Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Like { get; set; }
    }
}
