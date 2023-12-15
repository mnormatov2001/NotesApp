using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNotesCount
{
    public class GetNotesCountQueryValidator : AbstractValidator<GetNotesCountQuery>
    {
        public GetNotesCountQueryValidator()
        {
            RuleFor(query => query.UserId).NotEqual(Guid.Empty);
            RuleFor(query => query.GroupId).NotEqual(Guid.Empty);
        }
    }
}
