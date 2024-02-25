using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.DeleteNote;

internal class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, Guid>
{
    private readonly INotesDbContext _dbContext;

    public DeleteNoteCommandHandler(INotesDbContext dbContext) => 
        _dbContext = dbContext;

    public async Task<Guid> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
            var note = await _dbContext.Notes.FirstOrDefaultAsync(entity =>
                entity.Id == request.Id, cancellationToken);

            if (note == null || note.UserId != request.UserId)
                throw new NotFoundException(nameof(Note), request.Id);

            _dbContext.Notes.Remove(note);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return note.Id;
        }
}