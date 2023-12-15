using FluentValidation;

namespace Notes.Application.Groups.Queries.GetGroupsList
{
    public class GetGroupsListQueryValidator : AbstractValidator<GetGroupsListQuery>
    {
        public GetGroupsListQueryValidator()
        {
            RuleFor(query => query.UserId).NotEqual(Guid.Empty);
        }
    }
}
