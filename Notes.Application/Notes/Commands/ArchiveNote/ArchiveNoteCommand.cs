using MediatR;

namespace Notes.Application.Notes.Commands.ArchiveNote
{
    public class ArchiveNoteCommand: IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
