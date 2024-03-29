﻿using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Commands.CreateNote;
using System.ComponentModel.DataAnnotations;

namespace Notes.WebApi.Models;

public class CreateNoteDto : IMapWith<CreateNoteCommand>
{
#pragma warning disable CS8618

    [Required]
    public string Title { get; set; }
    public string? Content { get; set; }
    public Guid ParentNoteId { get; set; }
    public string? Icon { get; set; }
    public string? CoverImage { get; set; }
    public bool IsArchived { get; set; }
    public bool IsPublished { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateNoteDto, CreateNoteCommand>()
            .ForMember(cmd => cmd.Title,
                opt => opt.MapFrom(dto => dto.Title))
            .ForMember(cmd => cmd.Content,
                opt => opt.MapFrom(dto => dto.Content))
            .ForMember(cmd => cmd.ParentNoteId,
                opt => opt.MapFrom(dto => dto.ParentNoteId))
            .ForMember(cmd => cmd.Icon,
                opt => opt.MapFrom(dto => dto.Icon))
            .ForMember(cmd => cmd.CoverImage,
                opt => opt.MapFrom(dto => dto.CoverImage))
            .ForMember(cmd => cmd.IsArchived,
                opt => opt.MapFrom(dto => dto.IsArchived))
            .ForMember(cmd => cmd.IsPublished,
                opt => opt.MapFrom(dto => dto.IsPublished));
    }
}