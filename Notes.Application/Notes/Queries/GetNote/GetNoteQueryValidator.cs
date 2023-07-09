using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNote
{
    public class GetNoteQueryValidator : AbstractValidator<GetNoteQuery>
    {
        public GetNoteQueryValidator()
        {
            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);
            RuleFor(cmd => cmd.NoteId).NotEqual(Guid.Empty);
        }
    }
}
