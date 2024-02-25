using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands.ArchiveNote;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Commands.RestoreNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.DTOs;
using Notes.Application.Notes.Queries.GetAllNotes;
using Notes.Application.Notes.Queries.GetNote;
using Notes.Application.Notes.Queries.GetNotes;
using Notes.Application.Notes.Queries.GetNotesCount;
using Notes.Application.Notes.Queries.GetNotesTrash;
using Notes.Application.Notes.Queries.GetPublicNote;
using Notes.WebApi.Models;

namespace Notes.WebApi.Controllers;

[Produces("Application/json")]
[Route("notes")]
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
                Id = id,
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
                Id = id,
                UserId = UserId
            };

            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

    /// <summary>
    /// Archives the note
    /// </summary>
    /// <param name="id">Note id (guid)</param>
    /// <returns>Returns archived note id (guid)</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="404">If the requested note is not found</response>
    /// <response code="400">If the request is not validated</response>
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPut("archive")]
    [Authorize]
    public async Task<ActionResult<Guid>> ArchiveNote(Guid id)
    {
            var cmd = new ArchiveNoteCommand
            {
                Id = id,
                UserId = UserId
            };

            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

    /// <summary>
    /// Restores the note
    /// </summary>
    /// <param name="id">Note id (guid)</param>
    /// <returns>Returns restored note id (guid)</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="404">If the requested note is not found</response>
    /// <response code="400">If the request is not validated</response>
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPut("restore")]
    [Authorize]
    public async Task<ActionResult<Guid>> RestoreNote(Guid id)
    {
            var cmd = new RestoreNoteCommand
            {
                Id = id,
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
    [HttpPut("update")]
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
    /// <returns>Returns created note id (guid)</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="400">If the request is not validated</response>
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<Guid>> CreateNote([FromBody] CreateNoteDto createNoteDto)
    {
            var cmd = _mapper.Map<CreateNoteCommand>(createNoteDto);
            cmd.UserId = UserId;
            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

    /// <summary>
    /// Gets the number of child elements of a note
    /// </summary>
    /// <param name="parentNoteId">Id of parent note (guid)</param>
    /// <returns>Returns (int)</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="400">If the request is not validated</response>
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("children/count")]
    [Authorize]
    public async Task<ActionResult<int>> GetChildNotesCount(Guid parentNoteId)
    {
            var query = new GetNotesCountQuery
            {
                UserId = UserId,
                ParentNoteId = parentNoteId
            };
            var notesCount = await Mediator.Send(query);
            return Ok(notesCount);
        }

    /// <summary>
    /// Gets all child elements of a note
    /// </summary>
    /// <param name="parentNoteId">Id of parent note (guid)</param>
    /// <returns>Returns (NoteVm[])</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="400">If the request is not validated</response>
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("children")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<NoteVm>>> GetChildrenNotes(Guid parentNoteId)
    {
            var query = new GetNotesQuery
            {
                UserId = UserId,
                ParentNoteId = parentNoteId
            };
            var notes = await Mediator.Send(query);
            return Ok(notes);
        }

    /// <summary>
    /// Gets all notes
    /// </summary>
    /// <returns>Returns (NoteVm[])</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="400">If the request is not validated</response>
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("all")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<NoteVm>>> GetAllNotes()
    {
            var query = new GetAllNotesQuery()
            {
                UserId = UserId,
            };
            var notes = await Mediator.Send(query);
            return Ok(notes);
        }

    /// <summary>
    /// Gets notes trash
    /// </summary>
    /// <returns>Returns (NoteVm[])</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="400">If the request is not validated</response>
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("trash")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<NoteVm>>> GetNotesTrash()
    {
            var query = new GetNotesTrashQuery
            {
                UserId = UserId,
            };
            var notes = await Mediator.Send(query);
            return Ok(notes);
        }

    /// <summary>
    /// Gets public note by id
    /// </summary>
    /// <param name="id">Note id (guid)</param>
    /// <returns>Returns NoteVm</returns>
    /// <response code="200">Success</response>
    /// <response code="404">If the requested note is not found</response>
    /// <response code="400">If the request is not validated</response>
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("public/{id:guid}")]
    public async Task<ActionResult<NoteVm>> GetPublicNote(Guid id)
    {
            var query = new GetPublicNoteQuery
            {
                Id = id,
            };

            var noteVm = await Mediator.Send(query);
            return Ok(noteVm);
        }
}