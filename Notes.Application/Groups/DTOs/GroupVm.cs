using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;

namespace Notes.Application.Groups.DTOs
{
    public class GroupVm : IMapWith<Group>
    {
        #nullable disable
        public Guid Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GroupVm>()
                .ForMember(groupVm => groupVm.Id,
                    opt => opt.MapFrom(group => group.Id))
                .ForMember(groupVm => groupVm.Name,
                    opt => opt.MapFrom(group => group.Name));
        }
    }
}
