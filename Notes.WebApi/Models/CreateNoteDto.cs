using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Commands.CreateNote;

namespace Notes.WebApi.Models
{
    public class CreateNoteDto : IMapWith<CreateNoteCommand>
    {
        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateNoteDto, CreateNoteCommand>()
                .ForMember(cmd => cmd.NoteTitle,
                    opt => opt.MapFrom(dto => dto.NoteTitle))
                .ForMember(cmd => cmd.NoteContent,
                    opt => opt.MapFrom(dto => dto.NoteContent));
        }
    }
}
