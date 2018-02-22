using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.CommandStack.Resolvers;
using Xer.Delegator;
using Xer.Delegator.Registrations;
using Xer.Delegator.Resolvers;

namespace Xer.Cqrs.Extensions.Autofac
{
    internal class CqrsCommandBuilder : ICqrsCommandBuilder
    {
        private readonly ContainerBuilder _builder;

        internal CqrsCommandBuilder(ContainerBuilder builder)
        {
            _builder = builder;
        }

        internal ICqrsCommandBuilder AddCore()
        {
            _builder.Register(context =>
            {
                IMessageHandlerResolver[] handlers = context.Resolve<IEnumerable<CommandHandlerDelegateResolver>>().ToArray<IMessageHandlerResolver>();

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

        public ICqrsCommandBuilder ByInterface(params Assembly[] assemblies)
        {
            _builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(ICommandAsyncHandler<>))
                .AsImplementedInterfaces();

            _builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .AsImplementedInterfaces();

            _builder.Register(context =>
            {
                return new CommandHandlerDelegateResolver(
                    CompositeMessageHandlerResolver.Compose(
                        new ContainerCommandAsyncHandlerResolver(new ComponentContextAdapter(context)),
                        new ContainerCommandHandlerResolver(new ComponentContextAdapter(context))));
            }).AsSelf().SingleInstance();

            return this;
        }

        public ICqrsCommandBuilder ByAttribute(params Assembly[] assemblies)
        {
            _builder.RegisterAssemblyTypes(assemblies)
                .Where(CommandHandlerAttributeMethod.IsFoundInType)
                .AsSelf();

            _builder.Register(context =>
            {
                SingleMessageHandlerRegistration singleMessageHandlerRegistration = new SingleMessageHandlerRegistration();
                singleMessageHandlerRegistration.RegisterCommandHandlerAttributes(assemblies, context.Resolve);

                return new CommandHandlerDelegateResolver(
                    singleMessageHandlerRegistration.BuildMessageHandlerResolver());
            }).AsSelf().SingleInstance();

            return this;
        }
    }
}
