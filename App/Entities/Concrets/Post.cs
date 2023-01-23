namespace Indigo.Entities.Concrets
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }   
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
