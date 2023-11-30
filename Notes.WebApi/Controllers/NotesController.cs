using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.DTOs;
using Notes.Application.Notes.Queries.GetNote;
using Notes.Application.Notes.Queries.GetNotesCount;
using Notes.Application.Notes.Queries.GetNotesPage;
using Notes.WebApi.Models;

namespace Notes.WebApi.Controllers
{
    [Produces("Application/json")]
    [Route("[controller]")]
    public class NotesController : BaseApiController
    {
        private readonly IMapper _mapper;

        public NotesController(IMapper mapper) => 
            _mapper = mapper;

        /// <summary>
        /// Gets the note by id
        /// </summary>
        /// <param name="id">Note id (guid)</param>
        /// <returns>Returns NoteVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        /// <response code="404">If the requested note is not found</response>
        /// <response code="400">If the request is not validated</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<NoteVm>> GetNote(Guid id)
        {
            var query = new GetNoteQuery
            {
                NoteId = id,
                UserId = UserId
            };

            var noteVm = await Mediator.Send(query);
            return Ok(noteVm);
        }

        /// <summary>
        /// Deletes the note by id
        /// </summary>
        /// <param name="id">Note id (guid)</param>
        /// <returns>Returns deleted note id (guid)</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        /// <response code="404">If the requested note is not found</response>
        /// <response code="400">If the request is not validated</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<Guid>> DeleteNote(Guid id)
        {
            var cmd = new DeleteNoteCommand
            {
                NoteId = id,
                UserId = UserId
            };

            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        /// <summary>
        /// Updates the note
        /// </summary>
        /// <param name="updateNoteDto"></param>
        /// <returns>Returns updated note id (guid)</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        /// <response code="404">If the requested note is not found</response>
        /// <response code="400">If the request is not validated</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Guid>> UpdateNote([FromBody] UpdateNoteDto updateNoteDto)
        {
            var cmd = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
            cmd.UserId = UserId;
            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        /// <summary>
        /// Creates the note
        /// </summary>
        /// <param name="createNoteDto">CreateNoteDto</param>
        /// <returns>Returns created note id</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        /// <response code="400">If the request is not validated</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateNote([FromBody] CreateNoteDto createNoteDto)
        {
            var cmd = _mapper.Map<CreateNoteCommand>(createNoteDto);
            cmd.UserId = UserId;
            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        /// <summary>
        /// Gets notes page
        /// </summary>
        /// <param name="getNotesPageDto">GetNotesPageDto</param>
        /// <returns>Returns notes page (NotesPage)</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        /// <response code="400">If the request is not validated</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("page")]
        [Authorize]
        public async Task<ActionResult<NotesPage>> GetNotesPage(
            [FromQuery] GetNotesPageDto getNotesPageDto)
        {
            var query = _mapper.Map<GetNotesPageQuery>(getNotesPageDto);
            query.UserId = UserId;
            var notesPage = await Mediator.Send(query);
            return Ok(notesPage);
        }

        /// <summary>
        /// Gets notes count
        /// </summary>
        /// <returns>Returns notes count (int)</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        /// <response code="400">If the request is not validated</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("count/{groupId:guid}")]
        [Authorize]
        public async Task<ActionResult<int>> GetNotesCount(Guid groupId)
        {
            var query = new GetNotesCountQuery
            {
                UserId = UserId, 
                GroupId = groupId
            };
            var notesCount = await Mediator.Send(query);
            return Ok(notesCount);
        }
    }
}
