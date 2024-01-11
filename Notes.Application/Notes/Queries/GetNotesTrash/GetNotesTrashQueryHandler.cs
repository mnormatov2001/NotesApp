using AutoMapper;
using Notes.Application.Interfaces;
using MediatR;
using Notes.Application.Notes.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Notes.Application.Notes.Queries.GetNotesTrash
{
    internal class GetNotesTrashQueryHandler: IRequestHandler<GetNotesTrashQuery, IEnumerable<NoteVm>>
    {
        private readonly INotesDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetNotesTrashQueryHandler(INotesDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NoteVm>> Handle(GetNotesTrashQuery request,
            CancellationToken cancellationToken)
        {
            var notes = _dbContext.Notes
                .Where(entity => entity.UserId == request.UserId && entity.IsArchived);

            return await _mapper.ProjectTo<NoteVm>(notes).ToListAsync(cancellationToken);
        }
    }
}
