using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Groups.Commands.CreateGroup;
using Notes.Application.Groups.Commands.DeleteGroup;
using Notes.Application.Groups.Commands.UpdateGroup;
using Notes.Application.Groups.DTOs;
using Notes.Application.Groups.Queries.GetGroupsList;
using Notes.WebApi.Models;

namespace Notes.WebApi.Controllers
{
    [Produces("Application/json")]
    [Route("[controller]")]
    public class GroupsController : BaseApiController
    {
        private readonly IMapper _mapper;

        public GroupsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Creates the notes group
        /// </summary>
        /// <param name="groupName">Group name (string)</param>
        /// <returns>Returns created group id</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        /// <response code="400">If the request is not validated</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateGroup([FromQuery] string groupName)
        {
            var cmd = new CreateGroupCommand
            {
                UserId = UserId,
                GroupName = groupName
            };

            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        /// <summary>
        /// Updates the notes group
        /// </summary>
        /// <param name="updateGroupDto">UpdateGroupDto</param>
        /// <returns>Returns updated group id (guid)</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        /// <response code="404">If the requested group is not found</response>
        /// <response code="400">If the request is not validated</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Guid>> UpdateGroup([FromBody] UpdateGroupDto updateGroupDto)
        {
            var cmd = _mapper.Map<UpdateGroupCommand>(updateGroupDto);
            cmd.UserId = UserId;
            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the notes group by id
        /// </summary>
        /// <param name="id">Group id (guid)</param>
        /// <returns>Returns deleted group id (guid)</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        /// <response code="404">If the requested group is not found</response>
        /// <response code="400">If the request is not validated</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<Guid>> DeleteGroup(Guid id)
        {
            var cmd = new DeleteGroupCommand
            {
                GroupId = id,
                UserId = UserId
            };

            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        /// <summary>
        /// Gets notes list
        /// </summary>
        /// <returns>Returns list of notes groups (IList&lt;GroupVm&gt;)</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IList<GroupVm>>> GetGroupsList()
        {
            var query = new GetGroupsListQuery { UserId = UserId };
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
