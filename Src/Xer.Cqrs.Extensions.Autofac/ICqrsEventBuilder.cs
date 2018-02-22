using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsEventBuilder
    {
        ICqrsEventBuilder ByInterface(params Assembly[] assemblies);

        ICqrsEventBuilder ByAttribute(params Assembly[] assemblies);
    }
}
