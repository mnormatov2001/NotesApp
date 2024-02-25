using MediatR;

namespace Notes.Application.Notes.Commands.CreateNote;

public class CreateNoteCommand : IRequest<Guid>
{
#pragma warning disable CS8618
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
    public Guid ParentNoteId { get; set; }
    public string? Icon { get; set; }
    public string? CoverImage { get; set; }
    public bool IsArchived { get; set; }
    public bool IsPublished { get; set; }
}