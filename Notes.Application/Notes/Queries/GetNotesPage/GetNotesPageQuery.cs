using MediatR;
using Notes.Application.Notes.DTOs;

namespace Notes.Application.Notes.Queries.GetNotesPage
{
    public class GetNotesPageQuery : IRequest<NotesPage>
    {
        public Guid UserId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public SortKey SortKey { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}
