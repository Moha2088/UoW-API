
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



        #region Indexes


        builder.HasIndex(x => new { x.Name, x.From, x.To, x.State });
        builder.HasIndex(x => new { x.Name, x.From, x.To, x.State });
        builder.HasIndex(x => new { x.Name, x.From});
        builder.HasIndex(x => new { x.Name, x.To});
        builder.HasIndex(x => new { x.Name});

        builder.HasIndex(x => new {x.From, x.To, x.State});

        builder.HasIndex(x => new {x.To});

        builder.HasIndex(x => new{x.State});

        #endregion


        #region Relations

        builder.HasMany(x => x.Users)
            .WithOne(x => x.Project)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

    }
}
