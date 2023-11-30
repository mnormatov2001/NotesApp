using MediatR;

namespace Notes.Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommand : IRequest<Guid>
    {
        #nullable disable
        public Guid UserId { get; set; }
        public string GroupName { get; set; }
    }
}
