using System;
using System.Reflection;
using Xer.Cqrs.Extensions.Autofac;

namespace Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCqrs(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            if (assemblies.Length == 0)
            {
                throw new ArgumentException("No assemblies were provided.", nameof(assemblies));
            }

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
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return new CqrsBuilder(builder);
        }
    }
}
