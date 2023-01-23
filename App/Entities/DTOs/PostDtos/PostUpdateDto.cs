namespace Indigo.Entities.DTOs.PostDtos
{
    public class PostUpdateDto
    {
        public PostGetDto getDto { get; set; }
        public PostPostDto postDto { get; set; }
        public PostUpdateDto()
        {
            getDto = new PostGetDto();
            postDto = new PostPostDto();
        }
    }
}
