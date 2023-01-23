using FluentValidation;
using Indigo.Entities.DTOs.PostDtos;

namespace Indigo.Utilities.Validation.Fluent.Post
{
    public class PostPostDtoValidation: AbstractValidator<PostPostDto>
    {
        public PostPostDtoValidation()
        {
            RuleFor(x => x.Name).NotEmpty().NotEmpty();
            RuleFor(x => x.Description).NotEmpty().NotEmpty();
        }
    }
}
