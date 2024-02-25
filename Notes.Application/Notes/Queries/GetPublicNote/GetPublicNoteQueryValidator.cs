using FluentValidation;

namespace Notes.Application.Notes.Queries.GetPublicNote;

public class GetPublicNoteQueryValidator: AbstractValidator<GetPublicNoteQuery>
{
    public GetPublicNoteQueryValidator()
    {
            RuleFor(query => query.Id).NotEqual(Guid.Empty);
        }
}