using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Application.Notes.DTOs;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNote;

internal class GetNoteQueryHandler : IRequestHandler<GetNoteQuery, NoteVm>
{
    private readonly INotesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetNoteQueryHandler(INotesDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<NoteVm> Handle(GetNoteQuery request, CancellationToken cancellationToken)
    {
        var note = await _dbContext.Notes.FirstOrDefaultAsync(entity =>
            entity.Id == request.Id, cancellationToken);

        if (note == null || note.UserId != request.UserId)
            throw new NotFoundException(nameof(Note), request.Id);

        return _mapper.Map<NoteVm>(note);
    }
}