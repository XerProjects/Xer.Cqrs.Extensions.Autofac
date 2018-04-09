using Autofac;
using System.Collections.Generic;

namespace Xer.Cqrs.Extensions.Autofac
{
    /// <summary>
    /// Represents an adapter to <see cref="IComponentContext"/>.
    /// </summary>
    public class ComponentContextAdapter : CommandStack.Resolvers.IContainerAdapter,
                                           EventStack.Resolvers.IContainerAdapter
    {
        private readonly IComponentContext _context;

        public ComponentContextAdapter(IComponentContext context)
        {
            _context = context;
        }

        public T Resolve<T>() where T : class => _context.ResolveOptional<T>();

        public IEnumerable<T> ResolveMultiple<T>() where T : class => _context.ResolveOptional<IEnumerable<T>>();
    }
}
