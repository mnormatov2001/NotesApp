using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNotes;

public class GetNotesQueryValidator : AbstractValidator<GetNotesQuery>
{
    public GetNotesQueryValidator()
    {
        RuleFor(query => query.UserId).NotEqual(Guid.Empty);
    }
}