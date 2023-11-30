using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.DTOs;
using Notes.Application.Notes.Queries.GetNotesPage;

namespace Notes.WebApi.Models
{
    public class GetNotesPageDto : IMapWith<GetNotesPageQuery>
    {
        public Guid GroupId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        /// <summary>
        /// The key by which the collection of notes will be sorted
        /// </summary>
        /// <value>CreationDate</value>
        /// <value>EditDate</value>
        /// <value>Title</value>
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortKey SortKey { get; set; }

        /// <summary>
        /// Key that specifies the sort direction
        /// </summary>
        /// <value>Ascending</value>
        /// <value>Descending</value>
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortOrder SortOrder { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetNotesPageDto, GetNotesPageQuery>()
                .ForMember(query => query.GroupId,
                    opt => opt.MapFrom(dto => dto.GroupId))
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
