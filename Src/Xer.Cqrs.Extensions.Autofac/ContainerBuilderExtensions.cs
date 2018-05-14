using System.Reflection;
using Xer.Cqrs.Extensions.Autofac;

namespace Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCqrs(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterCqrsCore()
                .RegisterEventHandlers(select => 
                    select.ByInterface(assemblies)
                          .ByAttribute(assemblies))
                .RegisterCommandHandlers(select => 
                    select.ByInterface(assemblies)
                          .ByAttribute(assemblies));
                
            return builder;
        }

        public static ICqrsBuilder RegisterCqrsCore(this ContainerBuilder builder)
        {
            return new CqrsBuilder(builder);
        }
    }
}
