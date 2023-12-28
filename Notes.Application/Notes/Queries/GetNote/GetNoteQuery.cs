using MediatR;
using Notes.Application.Notes.DTOs;

namespace Notes.Application.Notes.Queries.GetNote
{
    public class GetNoteQuery : IRequest<NoteVm>
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
