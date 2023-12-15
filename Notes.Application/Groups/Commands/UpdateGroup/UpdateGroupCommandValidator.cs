using FluentValidation;

namespace Notes.Application.Groups.Commands.UpdateGroup
{
    public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
    {
        public UpdateGroupCommandValidator()
        {
            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.GroupId).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.GroupName).Must(str => 
                !string.IsNullOrWhiteSpace(str)).MaximumLength(250);
        }
    }
}
