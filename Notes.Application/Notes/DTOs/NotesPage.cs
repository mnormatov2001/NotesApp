namespace Notes.Application.Notes.DTOs
{
    public class NotesPage
    {
        public IList<NoteVm> Notes { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalNotesCount { get; set; }
        public int TotalPagesCount => (int)Math.Ceiling((double)TotalNotesCount / PageSize);
    }
}
