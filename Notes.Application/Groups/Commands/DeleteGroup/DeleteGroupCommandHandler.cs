using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Groups.Commands.DeleteGroup
{
    internal class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Guid>
    {
        private readonly INotesDbContext _dbContext;

        public DeleteGroupCommandHandler(INotesDbContext dbContext) => 
            _dbContext = dbContext;

        public async Task<Guid> Handle(DeleteGroupCommand request, 
            CancellationToken cancellationToken)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(entity =>
                entity.Id == request.GroupId, cancellationToken);

            if (group == null || group.UserId != request.UserId)
                throw new NotFoundException(nameof(Group), request.GroupId);

            _dbContext.Groups.Remove(group);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return group.Id;
        }
    }
}
