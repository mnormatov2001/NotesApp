using MediatR;
using Notes.Application.Notes.DTOs;

namespace Notes.Application.Notes.Queries.GetPublicNote;

public class GetPublicNoteQuery: IRequest<NoteVm>
{
    public Guid Id { get; set; }
}