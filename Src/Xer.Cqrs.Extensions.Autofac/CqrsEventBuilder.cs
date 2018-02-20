using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xer.Cqrs.EventStack;
using Xer.Cqrs.EventStack.Resolvers;
using Xer.Delegator;
using Xer.Delegator.Registrations;
using Xer.Delegator.Resolvers;

namespace Xer.Cqrs.Extensions.Autofac
{
    internal class CqrsEventBuilder : ICqrsEventBuilder
    {
        private readonly ContainerBuilder _builder;

        internal CqrsEventBuilder (ContainerBuilder builder)
        {
            _builder = builder;
        }

        public ICqrsEventBuilder AddEventHandlers(params Assembly[] assemblies)
        {
            _builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IEventAsyncHandler<>))
                .AsImplementedInterfaces();

            _builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .AsImplementedInterfaces();

            _builder.Register(context =>
            {
                return new EventHandlerDelegateResolver(
                    new ContainerEventHandlerResolver(new ComponentContextAdapter(context)));
            }).AsSelf().SingleInstance();

            return this;
        }

        public ICqrsEventBuilder AddEventHandlersByAttribute(params Assembly[] assemblies)
        {
            _builder.RegisterAssemblyTypes(assemblies)
                .Where(EventHandlerAttributeMethod.IsFoundInType)
                .AsSelf();

            _builder.Register(context =>
            {
                var handlerRegistration = new MultiMessageHandlerRegistration();
                handlerRegistration.RegisterEventHandlerAttributes(assemblies, context.Resolve);
                return new EventHandlerDelegateResolver(handlerRegistration.BuildMessageHandlerResolver());
            }).AsSelf().SingleInstance();

            return this;
        }

        internal ICqrsEventBuilder AddCore()
        {
            _builder.Register(context =>
            {
                IMessageHandlerResolver[] handlers = context.Resolve<IEnumerable<EventHandlerDelegateResolver>>()
                    .ToArray<IMessageHandlerResolver>();

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
