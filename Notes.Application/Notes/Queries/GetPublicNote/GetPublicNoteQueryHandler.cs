using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Application.Notes.DTOs;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetPublicNote;

internal class GetPublicNoteQueryHandler : IRequestHandler<GetPublicNoteQuery, NoteVm>
{
    private readonly INotesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetPublicNoteQueryHandler(INotesDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<NoteVm> Handle(GetPublicNoteQuery request, CancellationToken cancellationToken)
    {
        var note = await _dbContext.Notes.FirstOrDefaultAsync(entity =>
            entity.Id == request.Id && entity.IsPublished, cancellationToken);

        if (note == null)
            throw new NotFoundException(nameof(Note), request.Id);

        return _mapper.Map<NoteVm>(note);
    }
}