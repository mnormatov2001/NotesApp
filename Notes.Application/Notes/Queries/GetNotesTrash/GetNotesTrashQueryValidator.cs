using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNotesTrash;

public class GetNotesTrashQueryValidator : AbstractValidator<GetNotesTrashQuery>
{
    public GetNotesTrashQueryValidator()
    {
        RuleFor(query => query.UserId).NotEqual(Guid.Empty);
    }
}