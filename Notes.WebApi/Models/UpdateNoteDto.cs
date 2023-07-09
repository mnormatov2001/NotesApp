using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Commands.UpdateNote;

namespace Notes.WebApi.Models
{
    public class UpdateNoteDto : IMapWith<UpdateNoteCommand>
    {
        public Guid NoteId { get; set; }
        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateNoteDto, UpdateNoteCommand>()
                .ForMember(cmd => cmd.NoteId,
                    opt => opt.MapFrom(dto => dto.NoteId))
                .ForMember(cmd => cmd.NoteTitle,
                    opt => opt.MapFrom(dto => dto.NoteTitle))
                .ForMember(cmd => cmd.NoteContent,
                    opt => opt.MapFrom(dto => dto.NoteContent));
        }
    }
}
