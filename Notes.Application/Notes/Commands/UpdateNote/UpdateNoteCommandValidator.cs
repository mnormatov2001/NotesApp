using FluentValidation;

namespace Notes.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteCommandValidator()
        {
            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.NoteId).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.NoteTitle).Must(str => !string.IsNullOrWhiteSpace(str)).MaximumLength(250);
            RuleFor(cmd => cmd.NoteContent).Must(str => !string.IsNullOrWhiteSpace(str));
        }
    }
}
