﻿using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Commands.CreateNote;
using System.ComponentModel.DataAnnotations;

namespace Notes.WebApi.Models
{
    public class CreateNoteDto : IMapWith<CreateNoteCommand>
    {
        #nullable disable
        public Guid GroupId { get; set; }
        [Required]
        public string NoteTitle { get; set; }
        [Required]
        public string NoteContent { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateNoteDto, CreateNoteCommand>()
                .ForMember(cmd => cmd.GroupId,
                    opt => opt.MapFrom(dto => dto.GroupId))
                .ForMember(cmd => cmd.NoteTitle,
                    opt => opt.MapFrom(dto => dto.NoteTitle))
                .ForMember(cmd => cmd.NoteContent,
                    opt => opt.MapFrom(dto => dto.NoteContent));
        }
    }
}
