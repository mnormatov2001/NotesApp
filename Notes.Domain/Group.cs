namespace Notes.Domain
{
    public class Group
    {
        #nullable disable
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Note> Notes { get; set; }
    }
}
