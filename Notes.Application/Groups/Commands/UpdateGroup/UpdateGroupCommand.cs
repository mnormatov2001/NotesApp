using MediatR;

namespace Notes.Application.Groups.Commands.UpdateGroup
{
    public class UpdateGroupCommand : IRequest<Guid>
    {
        #nullable disable
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
