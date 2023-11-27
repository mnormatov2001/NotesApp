using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Notes.WebApi.Controllers
{
    [ApiController]
    [Route("notes.app/[controller]/[action]")]
    public abstract class BaseApiController : ControllerBase
    {
        #nullable disable
        private IMediator _mediator;
        protected IMediator Mediator => 
            _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

        internal Guid UserId => User.Identity is { IsAuthenticated: true }
            ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
            : Guid.Empty;
    }
}
