using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.UpdateNote
{
    internal class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, Guid>
    {
        private readonly INotesDbContext _dbContext;

        public UpdateNoteCommandHandler(INotesDbContext dbContext) => 
            _dbContext = dbContext;

        public async Task<Guid> Handle(UpdateNoteCommand request, 
            CancellationToken cancellationToken)
        {
            var note = await _dbContext.Notes.FirstOrDefaultAsync(entity => 
                entity.Id == request.NoteId, cancellationToken);

            if (note == null || note.UserId != request.UserId)
                throw new NotFoundException(nameof(Note), request.NoteId);

            note.Title = request.NoteTitle;
            note.Content = request.NoteContent;
            note.EditDate = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return note.Id;
        }
    }
}
