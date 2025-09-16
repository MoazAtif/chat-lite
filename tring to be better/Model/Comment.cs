namespace tring_to_be_better.Model
{
    public class Comment
    {

        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int PostId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
       
    }
}

