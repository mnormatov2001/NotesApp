using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;

namespace Notes.Application.Notes.Queries.GetNotesCount;

internal class GetNotesCountQueryHandler : IRequestHandler<GetNotesCountQuery, int>
{
    private readonly INotesDbContext _dbContext;

    public GetNotesCountQueryHandler(INotesDbContext dbContext) => 
        _dbContext = dbContext;

    public async Task<int> Handle(GetNotesCountQuery request, 
        CancellationToken cancellationToken)
    {
            return await _dbContext.Notes.CountAsync(note => 
                note.UserId == request.UserId &&
                note.ParentNoteId == request.ParentNoteId &&
                !note.IsArchived, cancellationToken);
        }
}