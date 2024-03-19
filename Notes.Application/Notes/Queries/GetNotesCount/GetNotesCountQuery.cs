using MediatR;

namespace Notes.Application.Notes.Queries.GetNotesCount;

public class GetNotesCountQuery : IRequest<int>
{
    public Guid UserId { get; set; }
    public Guid ParentNoteId { get; set; }
}