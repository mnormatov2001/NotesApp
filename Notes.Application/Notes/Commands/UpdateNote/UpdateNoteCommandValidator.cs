using FluentValidation;

namespace Notes.Application.Notes.Commands.UpdateNote;

public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator()
    {
            RuleFor(cmd => cmd.Id).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.Title)
                .Must(str => !string.IsNullOrWhiteSpace(str))
                .MaximumLength(250);
            RuleFor(cmd => cmd.Icon)
                .Must(str => str == null || !string.IsNullOrWhiteSpace(str))
                .MaximumLength(10);
        }
}