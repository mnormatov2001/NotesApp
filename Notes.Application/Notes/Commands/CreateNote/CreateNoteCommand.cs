﻿using MediatR;

namespace Notes.Application.Notes.Commands.CreateNote
{
    public class CreateNoteCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }
    }
}
