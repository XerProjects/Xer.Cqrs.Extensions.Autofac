using Autofac;
using System.Collections.Generic;

namespace Xer.Cqrs.Extensions.Autofac
{
    /// <summary>
    /// Represents an adapter to <see cref="IComponentContext"/>.
    /// </summary>
    public class ContainerBuilderProvider : CommandStack.Resolvers.IContainerAdapter,
                                            EventStack.Resolvers.IContainerAdapter
    {
        private readonly IComponentContext _context;

        public ContainerBuilderProvider(IComponentContext context)
        {
            _context = context;
        }

        public T Resolve<T>() where T : class => _context.Resolve<T>();

        public IEnumerable<T> ResolveMultiple<T>() where T : class => _context.Resolve<IEnumerable<T>>();
    }
}
