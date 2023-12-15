using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.CreateNote
{
    internal class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Guid>
    {
        private readonly INotesDbContext _dbContext;

        public CreateNoteCommandHandler(INotesDbContext dbContext) => 
            _dbContext = dbContext;

        public async Task<Guid> Handle(CreateNoteCommand request, 
            CancellationToken cancellationToken)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(entity =>
                entity.Id == request.GroupId, cancellationToken);

            if (group == null || group.UserId != request.UserId)
                throw new NotFoundException(nameof(Group), request.GroupId);

            var note = new Note
            {
                UserId = request.UserId,
                GroupId = request.GroupId,
                Id = Guid.NewGuid(),
                Title = request.NoteTitle,
                Content = request.NoteContent,
                CreationDate = DateTime.UtcNow
            };
            note.EditDate = note.CreationDate;

            await _dbContext.Notes.AddAsync(note, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return note.Id;
        }
    }
}
