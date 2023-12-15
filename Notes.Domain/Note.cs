﻿namespace Notes.Domain
{
    public class Note
    {
        #nullable disable
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}
