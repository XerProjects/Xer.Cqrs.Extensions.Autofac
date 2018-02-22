using System;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsBuilder
    {
        ICqrsBuilder AddCommandHandlers(Action<ICqrsCommandBuilder> builder = null);

        ICqrsBuilder AddEventHandlers(Action<ICqrsEventBuilder> builder = null);
    }
}
