using System;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsBuilder
    {
        ICqrsBuilder AddCommands(Action<ICqrsCommandBuilder> builder = null);

        ICqrsBuilder AddEvents(Action<ICqrsEventBuilder> builder = null);
    }
}
