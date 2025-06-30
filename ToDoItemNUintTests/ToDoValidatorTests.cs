using ToDoItemApi.Models.DTO;
using ToDoItemApi.Validators;

namespace ToDoItemNUintTests
{
    public class ToDoItemValidatorTests
    {
        [Test]
        public void Validator_Should_Fail_When_Title_Is_Empty()
        {
            var validator = new ToDoItemRequestValidator();
            var model = new ToDoItemRequestDto
            {
                Title = "",
                Description = "Some description"
            };

            var result = validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Title"));
        }


        [Test]
        public void Validator_Should_Pass_When_Title_Is_Not_Empty()
        {
            var validator = new ToDoItemRequestValidator();
            var model = new ToDoItemRequestDto
            {
                Title = "Valid Title",
                Description = "Some description"
            };

            var result = validator.Validate(model);

            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.Errors.Any(e => e.PropertyName == "Title"));
        }

    }
}
