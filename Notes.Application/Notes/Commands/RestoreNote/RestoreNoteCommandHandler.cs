using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.RestoreNote;

internal class RestoreNoteCommandHandler : IRequestHandler<RestoreNoteCommand, Guid>
{
    private readonly INotesDbContext _dbContext;

    public RestoreNoteCommandHandler(INotesDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Guid> Handle(RestoreNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _dbContext.Notes.FirstOrDefaultAsync(entity =>
            entity.Id == request.Id, cancellationToken);

        if (note == null || note.UserId != request.UserId)
            throw new NotFoundException(nameof(Note), request.Id);

        if (note.IsArchived == false)
            return note.Id;

        note.IsArchived = false;
        if (note.ParentNoteId != Guid.Empty)
        {
            var parent =
                await _dbContext.Notes.FirstOrDefaultAsync(entity => entity.Id == note.ParentNoteId,
                    cancellationToken);
            if (parent == null || parent.IsArchived)
                note.ParentNoteId = Guid.Empty;
        }

        await RecursiveRestore(note.Id);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return note.Id;

        async Task RecursiveRestore(Guid noteId)
        {
            var children = await _dbContext.Notes
                .Where(entity => entity.UserId == request.UserId &&
                                 entity.ParentNoteId == noteId && entity.IsArchived)
                .ToListAsync(cancellationToken);

            foreach (var child in children)
            {
                child.IsArchived = false;
                await RecursiveRestore(child.Id);
            }
        }
    }
}