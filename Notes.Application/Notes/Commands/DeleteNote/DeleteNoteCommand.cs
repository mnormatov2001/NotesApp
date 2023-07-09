using MediatR;

namespace Notes.Application.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public Guid NoteId { get; set; }
    }
}
