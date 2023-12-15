using MediatR;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Groups.Commands.CreateGroup
{
    internal class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Guid>
    {
        private readonly INotesDbContext _dbContext;

        public CreateGroupCommandHandler(INotesDbContext dbContext) => 
            _dbContext = dbContext;

        public async Task<Guid> Handle(CreateGroupCommand request, 
            CancellationToken cancellationToken)
        {
            var group = new Group
            {
                UserId = request.UserId,
                Id = Guid.NewGuid(),
                Name = request.GroupName
            };
            
            await _dbContext.Groups.AddAsync(group, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return group.Id;
        }
    }
}
