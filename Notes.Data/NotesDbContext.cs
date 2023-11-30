using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Data.EntityTypeConfigurations;
using Notes.Domain;

namespace Notes.Data
{
    public class NotesDbContext : DbContext, INotesDbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Group> Groups { get; set; }

        public NotesDbContext(DbContextOptions<NotesDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new NoteConfiguration());
            builder.ApplyConfiguration(new GroupConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
