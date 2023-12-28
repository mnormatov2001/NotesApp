using FluentValidation;

namespace Notes.Application.Notes.Queries.GetAllNotes
{
    public class GetAllNotesQueryValidator: AbstractValidator<GetAllNotesQuery>
    {
        public GetAllNotesQueryValidator()
        {
            RuleFor(query => query.UserId).NotEqual(Guid.Empty);
        }
    }
}
