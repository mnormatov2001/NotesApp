using MediatR;
using Notes.Application.Notes.DTOs;

namespace Notes.Application.Notes.Queries.GetAllNotes
{
    public class GetAllNotesQuery : IRequest<IEnumerable<NoteVm>>
    {
        public Guid UserId { get; set; }
    }
}
