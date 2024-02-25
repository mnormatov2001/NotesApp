using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.ArchiveNote;

public class ArchiveNoteCommandHandler: IRequestHandler<ArchiveNoteCommand, Guid>
{
    private readonly INotesDbContext _dbContext;

    public ArchiveNoteCommandHandler(INotesDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Guid> Handle(ArchiveNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _dbContext.Notes.FirstOrDefaultAsync(entity =>
            entity.Id == request.Id, cancellationToken);

        if (note == null || note.UserId != request.UserId)
            throw new NotFoundException(nameof(Note), request.Id);

        note.IsArchived = true;
        note.IsPublished = false;
        await RecursiveArchive(note.Id);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return note.Id;

        async Task RecursiveArchive(Guid noteId)
        {
            var children = await _dbContext.Notes
                .Where(entity => entity.UserId == request.UserId && entity.ParentNoteId == noteId)
                .ToListAsync(cancellationToken);

            foreach (var child in children)
            {
                child.IsArchived = true;
                child.IsPublished = false;
                await RecursiveArchive(child.Id);
            }
        }
    }
}