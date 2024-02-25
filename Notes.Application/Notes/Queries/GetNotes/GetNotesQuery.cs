using MediatR;
using Notes.Application.Notes.DTOs;

namespace Notes.Application.Notes.Queries.GetNotes;

public class GetNotesQuery: IRequest<IEnumerable<NoteVm>>
{
    public Guid UserId { get; set; }
    public Guid ParentNoteId { get; set; }
}