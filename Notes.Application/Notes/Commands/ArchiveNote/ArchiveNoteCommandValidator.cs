using FluentValidation;

namespace Notes.Application.Notes.Commands.ArchiveNote
{
    public class ArchiveNoteCommandValidator: AbstractValidator<ArchiveNoteCommand>
    {
        public ArchiveNoteCommandValidator()
        {
            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.Id).NotEqual(Guid.Empty);
        }
    }
}
