using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsEventHandlerSelector
    {
        ICqrsEventHandlerSelector ByInterface(params Assembly[] assemblies);
        ICqrsEventHandlerSelector ByInterface(Lifetime lifetime, params Assembly[] assemblies);
        ICqrsEventHandlerSelector ByAttribute(params Assembly[] assemblies);
        ICqrsEventHandlerSelector ByAttribute(Lifetime lifetime, params Assembly[] assemblies);
    }
}
