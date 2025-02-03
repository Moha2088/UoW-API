
using UoW_API.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UoW_API.Repositories.EntityTypeConfigurations;

public class ProjectEntitytypeConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasColumnType("varchar(20)")
            .IsRequired();




        #region Relations

        builder.HasMany(x => x.Users)
            .WithOne(x => x.Project)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

    }
}
