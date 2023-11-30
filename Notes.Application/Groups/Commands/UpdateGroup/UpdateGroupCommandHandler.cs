using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Groups.Commands.UpdateGroup
{
    internal class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Guid>
    {
        private readonly INotesDbContext _dbContext;

        public UpdateGroupCommandHandler(INotesDbContext dbContext) => 
            _dbContext = dbContext;

        public async Task<Guid> Handle(UpdateGroupCommand request, 
            CancellationToken cancellationToken)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(entity =>
            entity.Id == request.GroupId, cancellationToken);

            if (group == null || group.UserId != request.UserId)
                throw new NotFoundException(nameof(Group), request.GroupId);

            group.Name = request.GroupName;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return group.Id;
        }
    }
}
