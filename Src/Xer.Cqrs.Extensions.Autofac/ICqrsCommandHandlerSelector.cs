using System.Collections.Generic;
using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsCommandHandlerSelector
    {
        ICqrsCommandHandlerSelector ByInterface(Assembly assembly);
        ICqrsCommandHandlerSelector ByInterface(Assembly assembly, Lifetime lifetime);
        ICqrsCommandHandlerSelector ByInterface(IEnumerable<Assembly> assemblies);
        ICqrsCommandHandlerSelector ByInterface(IEnumerable<Assembly> assemblies, Lifetime lifetime);
        ICqrsCommandHandlerSelector ByAttribute(Assembly assembly);
        ICqrsCommandHandlerSelector ByAttribute(Assembly assembly, Lifetime lifetime);
        ICqrsCommandHandlerSelector ByAttribute(IEnumerable<Assembly> assemblies);
        ICqrsCommandHandlerSelector ByAttribute(IEnumerable<Assembly> assemblies, Lifetime lifetime);
    }
}
