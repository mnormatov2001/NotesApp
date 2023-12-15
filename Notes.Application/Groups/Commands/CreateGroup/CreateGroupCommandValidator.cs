using FluentValidation;

namespace Notes.Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupCommandValidator()
        {
            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.GroupName).Must(str => 
                !string.IsNullOrWhiteSpace(str)).MaximumLength(250);
        }
    }
}
