using MediatR;
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
            var note = new Note
            {
                UserId = request.UserId,
                Id = Guid.NewGuid(),
                Title = request.NoteTitle,
                Content = request.NoteContent,
                CreationDate = DateTime.Now
            };
            note.EditDate = note.CreationDate;

            await _dbContext.Notes.AddAsync(note, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return note.Id;
        }
    }
}
