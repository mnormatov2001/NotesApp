using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Application.Notes.DTOs;

namespace Notes.Application.Notes.Queries.GetNotesPage
{
    internal class GetNotesPageQueryHandler : IRequestHandler<GetNotesPageQuery, NotesPage>
    {
        private readonly INotesDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetNotesPageQueryHandler(INotesDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<NotesPage> Handle(GetNotesPageQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Notes.Where(note => note.UserId == request.UserId);

            query = request.SortKey switch
            {
                SortKey.CreationDate => 
                    request.SortOrder == SortOrder.Ascending
                    ? query.OrderBy(note => note.CreationDate)
                    : query.OrderByDescending(note => note.CreationDate),
                SortKey.EditDate => 
                    request.SortOrder == SortOrder.Ascending
                    ? query.OrderBy(note => note.EditDate)
                    : query.OrderByDescending(note => note.EditDate),
                SortKey.Title => 
                    request.SortOrder == SortOrder.Ascending
                    ? query.OrderBy(note => note.Title)
                    : query.OrderByDescending(note => note.Title),
                _ => query
            };

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.PageIndex > 0)
                query = query.Skip(request.PageIndex * request.PageSize);
            query = query.Take(request.PageSize);
            
            var items = await query.ProjectTo<NoteVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            return new NotesPage
            {
                Notes = items,
                TotalNotesCount = totalCount,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }
    }
}
