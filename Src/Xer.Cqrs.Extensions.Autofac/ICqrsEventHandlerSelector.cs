using System.Collections.Generic;
using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsEventHandlerSelector
    {
        ICqrsEventHandlerSelector ByInterface(Assembly assembly);
        ICqrsEventHandlerSelector ByInterface(Assembly assembly, Lifetime lifetime);
        ICqrsEventHandlerSelector ByInterface(IEnumerable<Assembly> assemblies);
        ICqrsEventHandlerSelector ByInterface(IEnumerable<Assembly> assemblies, Lifetime lifetime);
        ICqrsEventHandlerSelector ByAttribute(Assembly assembly);
        ICqrsEventHandlerSelector ByAttribute(Assembly assembly, Lifetime lifetime);
        ICqrsEventHandlerSelector ByAttribute(IEnumerable<Assembly> assemblies);
        ICqrsEventHandlerSelector ByAttribute(IEnumerable<Assembly> assemblies, Lifetime lifetime);
    }
}
