using Clean_Project_Backend.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Clean_Project_Backend.Repositories.EntityTypeConfigurations;

public class UserEntitytypeConfiguration : IEntityTypeConfiguration<User>
{

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasColumnType("varchar(20)");




        #region Relations

        builder.HasOne(x => x.Project)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.ProjectId);

        #endregion
    }
}
