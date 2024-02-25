using MediatR;

namespace Notes.Application.Notes.Commands.RestoreNote;

public class RestoreNoteCommand: IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
}