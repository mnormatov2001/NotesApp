using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Groups.Commands.UpdateGroup;

namespace Notes.WebApi.Models
{
    public class UpdateGroupDto : IMapWith<UpdateGroupCommand>
    {
        #nullable disable
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateGroupDto, UpdateGroupCommand>()
                .ForMember(cmd => cmd.GroupId,
                    opt => opt.MapFrom(dto => dto.GroupId))
                .ForMember(cmd => cmd.GroupName,
                    opt => opt.MapFrom(dto => dto.GroupName));
        }
    }
}
