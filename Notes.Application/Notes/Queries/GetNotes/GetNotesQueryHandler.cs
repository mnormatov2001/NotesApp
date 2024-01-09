using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Application.Notes.DTOs;

namespace Notes.Application.Notes.Queries.GetNotes
{
    internal class GetNotesQueryHandler: IRequestHandler<GetNotesQuery, IEnumerable<NoteVm>>
    {
        private readonly INotesDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetNotesQueryHandler(IMapper mapper, INotesDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NoteVm>> Handle(GetNotesQuery request,
            CancellationToken cancellationToken)
        {
            var notes = _dbContext.Notes
                .Where(entity => entity.ParentNoteId == request.ParentNoteId &&
                                 entity.UserId == request.UserId && !entity.IsArchived);

            return await _mapper.ProjectTo<NoteVm>(notes).ToListAsync(cancellationToken);
        }
    }
}
