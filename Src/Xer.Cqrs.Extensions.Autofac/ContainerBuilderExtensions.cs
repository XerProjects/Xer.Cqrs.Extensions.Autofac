using Autofac;
using System.Collections.Generic;
using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddCqrs(this ContainerBuilder builder, Assembly assembly)
        {
            return builder.AddCqrs(new[] { assembly });
        }

        public static ContainerBuilder AddCqrs(this ContainerBuilder builder, IEnumerable<Assembly> assemblies)
        {
            //builder.AddCqrsCore();

            return builder;
        }

        public static ICqrsBuilder AddCqrsCore(this ContainerBuilder builder)
        {
            return new CqrsBuilder(builder);
        }
    }
}
