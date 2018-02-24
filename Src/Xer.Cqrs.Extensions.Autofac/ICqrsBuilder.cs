using System;

namespace Xer.Cqrs.Extensions.Autofac
{
    public interface ICqrsBuilder
    {
        ICqrsBuilder RegisterCommandHandlers(Action<ICqrsCommandHandlerSelector> selector);

        ICqrsBuilder RegisterEventHandlers(Action<ICqrsEventHandlerSelector> selector);
    }
}
