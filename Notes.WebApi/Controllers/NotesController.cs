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
    [Route("notes.app/[controller]")]
    public class NotesController : BaseApiController
    {
        private readonly IMapper _mapper;

        public NotesController(IMapper mapper) => 
            _mapper = mapper;

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

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Guid>> UpdateNote([FromBody] UpdateNoteDto updateNoteDto)
        {
            var cmd = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
            cmd.UserId = UserId;
            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateNote([FromBody] CreateNoteDto createNoteDto)
        {
            var cmd = _mapper.Map<CreateNoteCommand>(createNoteDto);
            cmd.UserId = UserId;
            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        [HttpGet("page")]
        [Authorize]
        public async Task<ActionResult<NotesPage>> GetNotesPage([FromBody] GetNotesPageDto getNotesPageDto)
        {
            var query = _mapper.Map<GetNotesPageQuery>(getNotesPageDto);
            query.UserId = UserId;
            var notesPage = await Mediator.Send(query);
            return Ok(notesPage);
        }

        [HttpGet("count")]
        [Authorize]
        public async Task<ActionResult<int>> GetNotesCount()
        {
            var query = new GetNotesCountQuery { UserId = UserId };
            var notesCount = await Mediator.Send(query);
            return Ok(notesCount);
        }
    }
}
