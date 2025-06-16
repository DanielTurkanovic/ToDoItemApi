using FluentValidation;
using ToDoItemApi.Models.DTO;

namespace ToDoItemApi.Validators
{
    public class ToDoItemRequestValidator : AbstractValidator<ToDoItemRequestDto>
    {
        public ToDoItemRequestValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title is must be 100 characters or fewer");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description must be 200 characters or fewer.");
        }
    }
}
