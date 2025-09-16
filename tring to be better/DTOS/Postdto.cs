namespace tring_to_be_better.DTOS
{
    public class Postdto
    {
        public int Id { get; set; }
        public string Title{ get; set; }
        public string content{get; set; }
       
        public string UserName { get; set; }
        public List<commentdto> comments{ get; set; }
       
    }
}
