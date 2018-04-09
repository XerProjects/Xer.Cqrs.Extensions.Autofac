using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.EventStack;
using Xer.Delegator;
using Xer.Delegator.Registrations;
using Xer.Delegator.Resolvers;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Xer.Cqrs.Extensions.Autofac.Tests")]

namespace Xer.Cqrs.Extensions.Autofac
{
    internal class CqrsBuilder : ICqrsBuilder
    {
        private readonly ContainerBuilder _builder;
        private readonly CqrsCommandHandlerSelector _commandHandlerSelector;
        private readonly CqrsEventHandlerSelector _eventHandlerSelector;

        internal CqrsBuilder(ContainerBuilder builder)
        {
            _builder = builder;
            _commandHandlerSelector = new CqrsCommandHandlerSelector(_builder);
            _eventHandlerSelector = new CqrsEventHandlerSelector(_builder);
        }

        public ICqrsBuilder RegisterCommandHandlers(Action<ICqrsCommandHandlerSelector> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            // Select command handlers to register.
            selector(_commandHandlerSelector);

            _builder.Register(context =>
            {
                IMessageHandlerResolver[] handlers = context.Resolve<IEnumerable<CommandHandlerDelegateResolver>>().ToArray();

                if (handlers.Any())
                {
                    return handlers.Length > 1
                        ? new CommandDelegator(CompositeMessageHandlerResolver.Compose(handlers))
                        : new CommandDelegator(handlers[0]);
                }

                return new CommandDelegator(new SingleMessageHandlerRegistration().BuildMessageHandlerResolver());
            }).As<CommandDelegator>().SingleInstance();

            return this;
        }

        public ICqrsBuilder RegisterEventHandlers(Action<ICqrsEventHandlerSelector> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            // Select event handlers to register.
            selector(_eventHandlerSelector);
            
            _builder.Register(context =>
            {
                IMessageHandlerResolver[] handlers = context.Resolve<IEnumerable<EventHandlerDelegateResolver>>().ToArray();

                if (handlers.Any())
                {
                    return handlers.Length > 1
                        ? new EventDelegator(CompositeMessageHandlerResolver.Compose(handlers))
                        : new EventDelegator(handlers[0]);
                }

                return new EventDelegator(new MultiMessageHandlerRegistration().BuildMessageHandlerResolver());
            }).As<EventDelegator>().SingleInstance();

            return this;
        }
    }
}
