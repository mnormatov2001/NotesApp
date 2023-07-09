using System.Text.Json.Serialization;
using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.DTOs;
using Notes.Application.Notes.Queries.GetNotesPage;

namespace Notes.WebApi.Models
{
    public class GetNotesPageDto : IMapWith<GetNotesPageQuery>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortKey SortKey { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortOrder SortOrder { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetNotesPageDto, GetNotesPageQuery>()
                .ForMember(query => query.PageIndex,
                    opt => opt.MapFrom(dto => dto.PageIndex))
                .ForMember(query => query.PageSize,
                    opt => opt.MapFrom(dto => dto.PageSize))
                .ForMember(query => query.SortKey,
                    opt => opt.MapFrom(dto => dto.SortKey))
                .ForMember(query => query.SortOrder,
                    opt => opt.MapFrom(dto => dto.SortOrder));
        }
    }
}
