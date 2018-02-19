using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsCommandBuilder
    {
        ICqrsCommandBuilder AddCommandHandlers(params Assembly[] assemblies);

        ICqrsCommandBuilder AddCommandHandlersByAttribute(params Assembly[] assemblies);
    }
}
