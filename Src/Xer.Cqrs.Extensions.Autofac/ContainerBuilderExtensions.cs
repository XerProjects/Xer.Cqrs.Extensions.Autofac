using Autofac;
using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddCqrs(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.AddCqrsCore()
                .AddEvents(opt => opt
                    .AddEventHandlers(assemblies)
                    .AddEventHandlersByAttribute(assemblies))
                .AddCommands(opt => opt
                    .AddCommandHandlers(assemblies)
                    .AddCommandHandlersByAttribute(assemblies));
                
            return builder;
        }

        public static ICqrsBuilder AddCqrsCore(this ContainerBuilder builder)
        {
            return new CqrsBuilder(builder)
                .AddCommands()
                .AddEvents();
        }
    }
}
