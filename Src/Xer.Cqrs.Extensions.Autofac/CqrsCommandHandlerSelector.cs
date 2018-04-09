using Autofac;
using System;
using System.Linq;
using System.Reflection;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.CommandStack.Resolvers;
using Xer.Delegator;
using Xer.Delegator.Registrations;
using Xer.Delegator.Resolvers;

namespace Xer.Cqrs.Extensions.Autofac
{
    internal class CqrsCommandHandlerSelector : ICqrsCommandHandlerSelector
    {
        private readonly ContainerBuilder _builder;

        internal CqrsCommandHandlerSelector(ContainerBuilder builder)
        {
            _builder = builder;
        }
     
        public ICqrsCommandHandlerSelector ByInterface(params Assembly[] assemblies)
        {
            return ByInterface(Lifetime.PerDependency, assemblies);
        }

        public ICqrsCommandHandlerSelector ByInterface(Lifetime lifetime, params Assembly[] assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            if (assemblies.Length == 0)
            {
                throw new ArgumentException("No assemblies were provided.", nameof(assemblies));
            }

            var asyncHandlerRegistration = _builder.RegisterAssemblyTypes(assemblies.ToArray())
                .AsClosedTypesOf(typeof(ICommandAsyncHandler<>))
                .AsImplementedInterfaces();

            var syncHandlerRegistration = _builder.RegisterAssemblyTypes(assemblies.ToArray())
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .AsImplementedInterfaces();

            // Update registration if lifetime is not PerDependency.
            switch (lifetime)
            {
                case Lifetime.PerLifetimeScope:
                {
                    asyncHandlerRegistration.InstancePerLifetimeScope();
                    syncHandlerRegistration.InstancePerLifetimeScope();
                }
                break;
                case Lifetime.Singleton:
                {
                    asyncHandlerRegistration.SingleInstance();
                    syncHandlerRegistration.SingleInstance();
                }
                break;
            }

            _builder.Register(context =>
            {
                return new CommandHandlerDelegateResolver(
                    CompositeMessageHandlerResolver.Compose(
                        new ContainerCommandAsyncHandlerResolver(new ComponentContextAdapter(context)),
                        new ContainerCommandHandlerResolver(new ComponentContextAdapter(context))));
            }).AsSelf().SingleInstance();

            return this;
        }
       
        public ICqrsCommandHandlerSelector ByAttribute(params Assembly[] assemblies)
        {
            return ByAttribute(Lifetime.PerDependency, assemblies);
        }

        public ICqrsCommandHandlerSelector ByAttribute(Lifetime lifetime, params Assembly[] assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            if (assemblies.Length == 0)
            {
                throw new ArgumentException("No assemblies were provided.", nameof(assemblies));
            }

            var handlerRegistration = _builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(type => type.IsClass && !type.IsAbstract &&
                               CommandHandlerAttributeMethod.IsFoundInType(type))
                .AsSelf();

            // Update registration if lifetime is not PerDependency.
            switch (lifetime)
            {
                case Lifetime.PerLifetimeScope:
                {
                    handlerRegistration.InstancePerLifetimeScope();
                }
                break;
                case Lifetime.Singleton:
                {
                    handlerRegistration.SingleInstance();
                }
                break;
            }

            _builder.Register(context =>
            {
                SingleMessageHandlerRegistration singleMessageHandlerRegistration = new SingleMessageHandlerRegistration();
                singleMessageHandlerRegistration.RegisterCommandHandlerAttributes(assemblies, context.Resolve);
                return new CommandHandlerDelegateResolver(singleMessageHandlerRegistration.BuildMessageHandlerResolver());
            }).AsSelf().SingleInstance();

            return this;
        }
    }
}
