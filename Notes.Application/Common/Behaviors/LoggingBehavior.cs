using Serilog;
using MediatR;
using Notes.Application.Interfaces;

namespace Notes.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest
    : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehavior(ICurrentUserService currentUserService) =>
        _currentUserService = currentUserService;

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId;

        Log.Information("Notes Request: Type={Name} UserId={@UserId}",
            requestName, userId);

        return await next();
    }
}