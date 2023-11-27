using MediatR;

namespace Notes.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommand : IRequest<Guid>
    {
        #nullable disable
        public Guid UserId { get; set; }
        public Guid NoteId { get; set; }
        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }
    }
}
