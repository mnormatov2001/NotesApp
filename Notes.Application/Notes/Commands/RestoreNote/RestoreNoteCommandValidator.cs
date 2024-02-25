using FluentValidation;

namespace Notes.Application.Notes.Commands.RestoreNote;

public class RestoreNoteCommandValidator: AbstractValidator<RestoreNoteCommand>
{
    public RestoreNoteCommandValidator()
    {
            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.Id).NotEqual(Guid.Empty);
        }
}