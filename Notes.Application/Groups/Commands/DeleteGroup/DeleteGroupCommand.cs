using MediatR;

namespace Notes.Application.Groups.Commands.DeleteGroup
{
    public class DeleteGroupCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
