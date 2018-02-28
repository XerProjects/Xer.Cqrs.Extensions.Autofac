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
    internal class CqrsCommandHandlerSelector : ICqrsCommandHandlerSelector
    {
        private readonly ContainerBuilder _builder;

        internal CqrsCommandHandlerSelector(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public ICqrsCommandHandlerSelector ByInterface(Assembly assembly)
        {
            return ByInterface(assembly, Lifetime.PerDependency);
        }

        public ICqrsCommandHandlerSelector ByInterface(Assembly assembly, Lifetime lifetime)
        {
            return ByInterface(new[] { assembly }, lifetime);
        }

        public ICqrsCommandHandlerSelector ByInterface(IEnumerable<Assembly> assemblies)
        {
            return ByInterface(assemblies, Lifetime.PerDependency);
        }

        public ICqrsCommandHandlerSelector ByInterface(IEnumerable<Assembly> assemblies, Lifetime lifetime)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
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

        public ICqrsCommandHandlerSelector ByAttribute(Assembly assembly)
        {
            return ByAttribute(assembly, Lifetime.PerDependency);
        }

        public ICqrsCommandHandlerSelector ByAttribute(Assembly assembly, Lifetime lifetime)
        {
            return ByAttribute(new[] { assembly }, lifetime);
        }

        public ICqrsCommandHandlerSelector ByAttribute(IEnumerable<Assembly> assemblies)
        {
            return ByAttribute(assemblies, Lifetime.PerDependency);
        }

        public ICqrsCommandHandlerSelector ByAttribute(IEnumerable<Assembly> assemblies, Lifetime lifetime)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
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
