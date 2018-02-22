using System.Reflection;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsCommandBuilder
    {
        ICqrsCommandBuilder ByInterface(params Assembly[] assemblies);

        ICqrsCommandBuilder ByAttribute(params Assembly[] assemblies);
    }
}
