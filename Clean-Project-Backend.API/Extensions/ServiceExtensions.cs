using Clean_Project_Backend.Repositories.Repository.Interfaces;
using Scrutor;

namespace Clean_Project_Backend.API.Extensions;

public static class ServiceExtensions
{


    public static void RegisterServices(this IServiceCollection collection)
    {
        var repositoryAssembly = typeof(IUserRepository).Assembly;

        collection.Scan(scan =>
            scan.FromAssemblies(repositoryAssembly)
                .AddClasses(classes => classes.Where(c =>
                    c.Name.EndsWith("Repository") ||
                    c.Name.EndsWith("Service")))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        //collection.AddAutoMapper(typeof(AutoMapperProfile));
    }
}
