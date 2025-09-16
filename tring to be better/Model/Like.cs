namespace tring_to_be_better.Model
{
    public class Like
    {
        public int UserId { get; set; }
        public int PostId { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }

    }
}
