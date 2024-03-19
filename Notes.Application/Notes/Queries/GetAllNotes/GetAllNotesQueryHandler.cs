using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Application.Notes.DTOs;

namespace Notes.Application.Notes.Queries.GetAllNotes;

internal class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, IEnumerable<NoteVm>>
{
    private readonly INotesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllNotesQueryHandler(INotesDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NoteVm>> Handle(GetAllNotesQuery request,
        CancellationToken cancellationToken)
    {
        var notes = _dbContext.Notes
            .Where(entity => entity.UserId == request.UserId && !entity.IsArchived);

        return await _mapper.ProjectTo<NoteVm>(notes).ToListAsync(cancellationToken);
    }
}