using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNotesCount;

public class GetNotesCountQueryValidator : AbstractValidator<GetNotesCountQuery>
{
    public GetNotesCountQueryValidator()
    {
            RuleFor(cmd => cmd.UserId).NotEqual(Guid.Empty);
        }
}