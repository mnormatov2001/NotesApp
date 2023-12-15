using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Groups.DTOs;
using Notes.Application.Interfaces;

namespace Notes.Application.Groups.Queries.GetGroupsList
{
    internal class GetGroupsListQueryHandler : IRequestHandler<GetGroupsListQuery, IList<GroupVm>>
    {
        private readonly INotesDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetGroupsListQueryHandler(INotesDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IList<GroupVm>> Handle(GetGroupsListQuery request, 
            CancellationToken cancellationToken)
        {
            return await _dbContext.Groups
                .Where(group => group.UserId == request.UserId)
                .ProjectTo<GroupVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
