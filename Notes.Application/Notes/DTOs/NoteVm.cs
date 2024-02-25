using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;

namespace Notes.Application.Notes.DTOs;

public class NoteVm : IMapWith<Note>
{
#pragma warning disable CS8618

    public Guid Id { get; set; }
    public Guid ParentNoteId { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime EditDate { get; set; }
    public string? Icon { get; set; }
    public string? CoverImage { get; set; }
    public bool IsArchived { get; set; }
    public bool IsPublished { get; set; }

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
                    opt => opt.MapFrom(note => note.EditDate))
                .ForMember(noteVm => noteVm.Icon,
                    opt => opt.MapFrom(note => note.Icon))
                .ForMember(noteVm => noteVm.CoverImage,
                    opt => opt.MapFrom(note => note.CoverImage))
                .ForMember(noteVm => noteVm.IsArchived,
                    opt => opt.MapFrom(note => note.IsArchived))
                .ForMember(noteVm => noteVm.IsPublished,
                    opt => opt.MapFrom(note => note.IsPublished))
                .ForMember(noteVm => noteVm.ParentNoteId,
                    opt => opt.MapFrom(note => note.ParentNoteId));
        }
}