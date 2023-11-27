﻿using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;

namespace Notes.Application.Notes.DTOs
{
    public class NoteVm : IMapWith<Note>
    {
        #nullable disable
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Note, NoteVm>()
                .ForMember(noteVm => noteVm.Id,
                    opt => opt.MapFrom(note => note.Id))
                .ForMember(noteVm => noteVm.Title,
                    opt => opt.MapFrom(note => note.Title))
                .ForMember(noteVm => noteVm.Content,
                    opt => opt.MapFrom(note => note.Content))
                .ForMember(noteVm => noteVm.CreationDate,
                    opt => opt.MapFrom(note => note.CreationDate))
                .ForMember(noteVm => noteVm.EditDate,
                    opt => opt.MapFrom(note => note.EditDate));
        }
    }
}
