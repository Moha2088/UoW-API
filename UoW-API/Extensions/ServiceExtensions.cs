using UoW_API.Repositories.Repository.Interfaces;
using Scrutor;
using AutoMapper;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using UoW_API.Repositories.UnitOfWork;
using UoW_API.Services.Interfaces;
using UoW_API.Repositories.Entities.MapperProfile;

namespace UoW_API.Extensions;

public static class ServiceExtensions
{


    public static void RegisterServices(this IServiceCollection collection, WebApplicationBuilder builder)
    {
        var repositoryAssembly = typeof(IUserRepository).Assembly;
        var serviceAssembly = typeof(IUserService).Assembly;

        collection.Scan(scan =>
            scan.FromAssemblies(repositoryAssembly, serviceAssembly)
                .AddClasses(classes => classes.Where(c =>
                    c.Name.EndsWith("Repository") ||
                    c.Name.EndsWith("Service")))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        collection.AddScoped<IUnitOfWork, UnitOfWork>();

        #region Redis

        collection.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = builder.Configuration["ConnectionStrings:Redis"];
            opt.InstanceName = "RedisCache";
        });

        #endregion
    }
}
