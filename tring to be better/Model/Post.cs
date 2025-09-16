namespace tring_to_be_better.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public int UserId { get; set; }
       

        public User User { get; set; }
        
        public List<Comment> Comments { get; set; }
        public List<Like> Like { get; set; }


    }
}
