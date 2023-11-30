using MediatR;
using Notes.Application.Groups.DTOs;

namespace Notes.Application.Groups.Queries.GetGroupsList
{
    public class GetGroupsListQuery : IRequest<IList<GroupVm>>
    {
        public Guid UserId { get; set; }
    }
}
