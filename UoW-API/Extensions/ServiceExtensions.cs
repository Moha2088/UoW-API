using UoW_API.Repositories.Repository.Interfaces;
using Scrutor;
using AutoMapper;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using UoW_API.Repositories.UnitOfWork;

namespace UoW_API.Extensions;

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

        collection.AddScoped<IUnitOfWork, UnitOfWork>();

        collection.AddAutoMapper(typeof(Program));
    }
}
