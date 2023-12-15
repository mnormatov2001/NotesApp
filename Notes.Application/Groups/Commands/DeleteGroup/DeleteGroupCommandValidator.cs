using FluentValidation;

namespace Notes.Application.Groups.Commands.DeleteGroup
{
    public class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
    {
        public DeleteGroupCommandValidator()
        {
            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.GroupId).NotEqual(Guid.Empty);
        }
    }
}
