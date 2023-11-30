using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Domain;

namespace Notes.Data.EntityTypeConfigurations
{
    internal class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(group => group.Id);
            builder.HasIndex(group => group.Id).IsUnique();
            builder.Property(group => group.Name).HasMaxLength(250);
        }
    }
}
