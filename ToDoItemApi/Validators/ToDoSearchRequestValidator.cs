using FluentValidation;
using ToDoItemApi.Models.DTO;

namespace ToDoItemApi.Validators
{
    public class ToDoSearchRequestValidator : AbstractValidator<ToDoSearchRequestDto>
    {
        public ToDoSearchRequestValidator()
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Title) || !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("At least one of Title or Description must be provided.");

            RuleFor(x => x.Title)
                .MaximumLength(100)
                .WithMessage("Title must be 100 characters or fewer.");

            RuleFor(x => x.Description)
                .MaximumLength(200)
                .WithMessage("Description must be 200 characters or fewer.");
        }
    }
}
