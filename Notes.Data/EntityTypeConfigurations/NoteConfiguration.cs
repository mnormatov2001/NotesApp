using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Domain;

namespace Notes.Data.EntityTypeConfigurations
{
    internal class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(note => note.Id);
            builder.HasIndex(note => note.Id).IsUnique();
            builder.Property(note => note.Icon).IsRequired(false).HasMaxLength(10);
            builder.HasIndex(note => note.ParentNoteId).IsUnique(false);
            builder.Property(note => note.CoverImage).IsRequired(false);
            builder.Property(note => note.Content).IsRequired(false);
            builder.Property(note => note.Title).HasMaxLength(250);
        }
    }
}
