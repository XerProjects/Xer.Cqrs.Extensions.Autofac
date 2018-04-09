using Autofac;
using System;
using System.Linq;
using System.Reflection;
using Xer.Cqrs.EventStack;
using Xer.Cqrs.EventStack.Resolvers;
using Xer.Delegator;
using Xer.Delegator.Registrations;

namespace Xer.Cqrs.Extensions.Autofac
{
    internal class CqrsEventHandlerSelector : ICqrsEventHandlerSelector
    {
        private readonly ContainerBuilder _builder;

        internal CqrsEventHandlerSelector (ContainerBuilder builder)
        {
            _builder = builder;
        }

        public ICqrsEventHandlerSelector ByInterface(params Assembly[] assemblies)
        {
            return ByInterface(Lifetime.PerDependency, assemblies);
        }

        public ICqrsEventHandlerSelector ByInterface(Lifetime lifetime, params Assembly[] assemblies)
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
                .AsClosedTypesOf(typeof(IEventAsyncHandler<>))
                .AsImplementedInterfaces();

            var syncHandlerRegistration = _builder.RegisterAssemblyTypes(assemblies.ToArray())
                .AsClosedTypesOf(typeof(IEventHandler<>))
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
                return new EventHandlerDelegateResolver(new ContainerEventHandlerResolver(new ComponentContextAdapter(context)));
            }).AsSelf().SingleInstance();

            return this;
        }
        
        public ICqrsEventHandlerSelector ByAttribute(params Assembly[] assemblies)
        {
            return ByAttribute(Lifetime.PerDependency, assemblies);
        }
        
        public ICqrsEventHandlerSelector ByAttribute(Lifetime lifetime, params Assembly[] assemblies)
        {
            if (assemblies == null)
            {
                throw new System.ArgumentNullException(nameof(assemblies));
            }

            if (assemblies.Length == 0)
            {
                throw new ArgumentException("No assemblies were provided.", nameof(assemblies));
            }

            var attributeHandlerRegistration = _builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(type => type.IsClass && !type.IsAbstract &&
                               EventHandlerAttributeMethod.IsFoundInType(type))
                .AsSelf();

            // Update registration if lifetime is not PerDependency.
            switch (lifetime)
            {
                case Lifetime.PerLifetimeScope:
                {
                    attributeHandlerRegistration.InstancePerLifetimeScope();
                }
                break;
                case Lifetime.Singleton:
                {
                    attributeHandlerRegistration.SingleInstance();
                }
                break;
            }

            _builder.Register(context =>
            {
                var handlerRegistration = new MultiMessageHandlerRegistration();
                handlerRegistration.RegisterEventHandlerAttributes(assemblies, context.Resolve);
                return new EventHandlerDelegateResolver(handlerRegistration.BuildMessageHandlerResolver());
            }).AsSelf().SingleInstance();

            return this;
        }
    }
}
