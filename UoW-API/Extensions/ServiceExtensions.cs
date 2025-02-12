using UoW_API.Repositories.Repository.Interfaces;
using Scrutor;
using UoW_API.Repositories.UnitOfWork.Interfaces;
using UoW_API.Repositories.UnitOfWork;
using UoW_API.Services.Interfaces;
using StackExchange.Redis;
using Azure.Security.KeyVault.Secrets;

namespace UoW_API.Extensions;

public static class ServiceExtensions
{


    public static void RegisterServices(this IServiceCollection collection, WebApplicationBuilder builder, SecretClient client)
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

        var redisConnectionString = client.GetSecret("REDIS-CONNECTION").Value.Value;

        collection.AddStackExchangeRedisCache(opt =>
        {

            opt.Configuration = builder.Configuration.GetConnectionString("Redis");
        });

        #endregion

        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    }
}
