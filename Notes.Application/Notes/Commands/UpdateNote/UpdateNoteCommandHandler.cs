using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.UpdateNote;

internal class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, Guid>
{
    private readonly INotesDbContext _dbContext;

    public UpdateNoteCommandHandler(INotesDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Guid> Handle(UpdateNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = await _dbContext.Notes.FirstOrDefaultAsync(entity =>
            entity.Id == request.Id, cancellationToken);

        if (note == null || note.UserId != request.UserId)
            throw new NotFoundException(nameof(Note), request.Id);

        note.Icon = request.Icon;
        note.Title = request.Title;
        note.Content = request.Content;
        note.EditDate = DateTime.UtcNow;
        note.CoverImage = request.CoverImage;
        note.IsArchived = request.IsArchived;
        note.IsPublished = request.IsPublished;
        note.ParentNoteId = request.ParentNoteId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return note.Id;
    }
}