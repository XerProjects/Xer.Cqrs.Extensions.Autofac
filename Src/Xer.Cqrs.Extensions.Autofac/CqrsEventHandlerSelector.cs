using Autofac;
using System.Collections.Generic;
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

        public ICqrsEventHandlerSelector ByInterface(Assembly assembly)
        {
            return ByInterface(assembly, Lifetime.PerDependency);
        }

        public ICqrsEventHandlerSelector ByInterface(Assembly assembly, Lifetime lifetime)
        {
            return ByInterface(new[] { assembly }, lifetime);
        }

        public ICqrsEventHandlerSelector ByInterface(IEnumerable<Assembly> assemblies)
        {
            return ByInterface(assemblies, Lifetime.PerDependency);
        }

        public ICqrsEventHandlerSelector ByInterface(IEnumerable<Assembly> assemblies, Lifetime lifetime)
        {
            if (assemblies == null)
            {
                throw new System.ArgumentNullException(nameof(assemblies));
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
        
        public ICqrsEventHandlerSelector ByAttribute(Assembly assembly)
        {
            return ByAttribute(assembly, Lifetime.PerDependency);
        }
        
        public ICqrsEventHandlerSelector ByAttribute(Assembly assembly, Lifetime lifetime)
        {
            return ByAttribute(new[] { assembly }, lifetime);
        }

        public ICqrsEventHandlerSelector ByAttribute(IEnumerable<Assembly> assemblies)
        {
            return ByAttribute(assemblies, Lifetime.PerDependency);
        }

        public ICqrsEventHandlerSelector ByAttribute(IEnumerable<Assembly> assemblies, Lifetime lifetime)
        {
            if (assemblies == null)
            {
                throw new System.ArgumentNullException(nameof(assemblies));
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
