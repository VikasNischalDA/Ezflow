using FluentValidation;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using StructureMap;

namespace LeadManagementSystem.API
{
    public class ApplicationRegistry : Registry
    {
        public ApplicationRegistry(IConfiguration configuration)
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory
                    (assembly => assembly.GetName().Name.StartsWith("LeadManagementSystem."));
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IValidator<>));

            }
            );
            For<IConfiguration>().Singleton();
            For<IMediator>().Use<Mediator>();
            For<IActionContextAccessor>().Use<ActionContextAccessor>();
            For<IServiceProviderIsService>().Use<EmptyServiceProviderIsService>();

            var handlerType = For(typeof(IRequestHandler<,>));
            handlerType.DecorateAllWith(typeof(ValidatorHandler<,>));
        }
    }

    public class EmptyServiceProviderIsService : IServiceProviderIsService
    {
        public bool IsService(Type serviceType) { return false; }
    }
}
