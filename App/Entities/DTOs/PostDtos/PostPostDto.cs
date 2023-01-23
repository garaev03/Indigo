namespace Indigo.Entities.DTOs.PostDtos
{
    public class PostPostDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
