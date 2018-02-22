using Autofac;
using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddCqrs(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.AddCqrsCore()
                .AddEventHandlers(opt => opt
                    .ByInterface(assemblies)
                    .ByAttribute(assemblies))
                .AddCommandHandlers(opt => opt
                    .ByInterface(assemblies)
                    .ByAttribute(assemblies));
                
            return builder;
        }

        public static ICqrsBuilder AddCqrsCore(this ContainerBuilder builder)
        {
            return new CqrsBuilder(builder)
                .AddCommandHandlers()
                .AddEventHandlers();
        }
    }
}
