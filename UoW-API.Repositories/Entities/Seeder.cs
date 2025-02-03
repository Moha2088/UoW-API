
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UoW_API.Repositories.Data;
using UoW_API.Repositories.Entities;

namespace UoW_API.Repositories.Entities;

public class Seeder
{
    public static async void Seed(IServiceProvider serviceProvider)
    {

        using (var context = new DataContext(
                   serviceProvider.GetRequiredService<
                       DbContextOptions<DataContext>>()))
        {

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Name = "Mohamed",
                        Email = "mo@mo.com"
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }

}