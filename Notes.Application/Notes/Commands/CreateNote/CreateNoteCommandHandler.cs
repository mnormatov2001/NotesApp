using MediatR;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.CreateNote;

internal class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Guid>
{
    private readonly INotesDbContext _dbContext;

    public CreateNoteCommandHandler(INotesDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Guid> Handle(CreateNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = new Note
        {
            Id = Guid.NewGuid(),
            Icon = request.Icon,
            Title = request.Title,
            Content = request.Content,
            UserId = request.UserId,
            CoverImage = request.CoverImage,
            IsArchived = request.IsArchived,
            IsPublished = request.IsPublished,
            ParentNoteId = request.ParentNoteId,
            CreationDate = DateTime.UtcNow,
        };
        note.EditDate = note.CreationDate;

        await _dbContext.Notes.AddAsync(note, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return note.Id;
    }
}