using System;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsBuilder
    {
        ICqrsBuilder AddCommands(Action<ICqrsCommandBuilder> builder);

        ICqrsBuilder AddEvents(Action<ICqrsEventBuilder> builder);
    }
}
