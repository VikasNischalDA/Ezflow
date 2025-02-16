using FluentValidation;
using LeadManagementSystem.API;
using LeadManagementSystem.API.AutoMapper;
using LeadManagementSystem.API.Controllers;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Data;
using LeadManagementSystem.Logic.Handler;
using LeadManagementSystem.Providers.HttpProvider;
using LeadManagementSystem.Providers.Interface;
using LeadManagementSystem.Test.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StructureMap;

public class TestDependencyResolver
{
    public static IServiceProvider ServiceProvider { get; private set; }
    public static IConfiguration Configuration { get; }

    static TestDependencyResolver()
    {
        var services = new ServiceCollection();

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration configuration = configurationBuilder.Build();
        services.AddSingleton<IConfiguration>(configuration);

        services.AddHttpClient("XmlClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7224");
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(LeadRequest).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(RegisterLeadHandler).Assembly);
        });

        services.AddTransient<IValidator<LeadRequest>, LeadManagementSystem.Logic.Handler.LeadValidationHandler>();

        var lesProviderMock = ExternaLesProviderMock.CreateMock();
        var dbsProviderMock = ExternalDbsProviderMock.CreateMock();
        var dalasProviderMock = ExternalDalasProviderMock.CreateMock();

        services.AddTransient(_ => lesProviderMock.Object);
        services.AddTransient(_ => dbsProviderMock.Object);
        services.AddTransient(_ => dalasProviderMock.Object);
        services.Replace(ServiceDescriptor.Scoped<IProvider<DbsRequest, DbsServiceCall>>(_ => ExternalDbsProviderMock.CreateMock().Object));
        services.AddScoped<IDalasProvider, ExternalDalasProvider>();
        services.AddAutoMapper(typeof(AutoMapperProfile));


        var connectionString = configuration["Database:ConnectionString"];
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
        services.AddTransient<LeadController>();

        var container = new Container();
        container.Configure(config =>
        {
            config.AddRegistry(new ApplicationRegistry(configuration));
            config.Populate(services);
        });

        ServiceProvider = container.GetInstance<IServiceProvider>();
        DatabaseSeeder.SeedDatabase(ServiceProvider);
    }

}