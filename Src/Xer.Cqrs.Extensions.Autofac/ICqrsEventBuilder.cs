using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsEventBuilder
    {
        ICqrsEventBuilder AddEventHandlers(params Assembly[] assemblies);

        ICqrsEventBuilder AddEventHandlersByAttribute(params Assembly[] assemblies);
    }
}
