using System.Collections.Generic;
using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsCommandHandlerSelector
    {
        ICqrsCommandHandlerSelector ByInterface(params Assembly[] assemblies);
        ICqrsCommandHandlerSelector ByInterface(Lifetime lifetime, params Assembly[] assemblies);
        ICqrsCommandHandlerSelector ByAttribute(params Assembly[] assemblies);
        ICqrsCommandHandlerSelector ByAttribute(Lifetime lifetime, params Assembly[] assemblies);
    }
}
