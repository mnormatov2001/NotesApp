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
            builder.HasIndex(note => note.GroupId).IsUnique(false);
            builder.Property(note => note.Title).HasMaxLength(250);
            builder.HasOne<Group>()
                .WithMany(group => group.Notes)
                .HasForeignKey(note => note.GroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
