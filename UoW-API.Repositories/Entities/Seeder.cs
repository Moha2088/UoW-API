
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
                        Name = "Mo",
                        Email = "mo@mo.com",
                        Password = "Mo123"
                    }
                );
            }

            if (!context.Projects.Any())
            {
                context.Projects.AddRange(
                    new Project
                    {
                        Name = "Backend Ecommerce",
                        Description = "A backend project for an Ecommerce website, with the implementation of an API, database and Cloud Service integration",
                        From = DateTimeOffset.Now.AddMinutes(5),
                        To = DateTimeOffset.Now.AddMonths(1),
                        State = Enums.CurrentState.PENDING
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }

}