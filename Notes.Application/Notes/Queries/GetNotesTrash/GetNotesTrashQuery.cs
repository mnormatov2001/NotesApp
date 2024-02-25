using MediatR;
using Notes.Application.Notes.DTOs;

namespace Notes.Application.Notes.Queries.GetNotesTrash;

public class GetNotesTrashQuery : IRequest<IEnumerable<NoteVm>>
{
    public Guid UserId { get; set; }
}