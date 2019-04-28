using Autofac;
using System;
using System.Linq;
using System.Reflection;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.CommandStack.Extensions.Attributes;
using Xer.Cqrs.CommandStack.Resolvers;
using Xer.Delegator;
using Xer.Delegator.Registration;
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
                throw new ArgumentException("No command handler assemblies were provided.", nameof(assemblies));
            }
            
            Assembly[] distinctAssemblies = assemblies.Distinct().ToArray();

            var asyncHandlerRegistration = _builder.RegisterAssemblyTypes(distinctAssemblies)
                .AsClosedTypesOf(typeof(ICommandAsyncHandler<>))
                .AsImplementedInterfaces();

            var syncHandlerRegistration = _builder.RegisterAssemblyTypes(distinctAssemblies)
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
                var c = context.Resolve<IComponentContext>();
                var adapter = new ComponentContextAdapter(c);
                return new CommandHandlerDelegateResolver(
                    CompositeMessageHandlerResolver.Compose(
                        new ContainerCommandAsyncHandlerResolver(adapter),
                        new ContainerCommandHandlerResolver(adapter)));
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
                throw new ArgumentException("No command handler assemblies were provided.", nameof(assemblies));
            }
            
            Assembly[] distinctAssemblies = assemblies.Distinct().ToArray();

            var handlerRegistration = _builder.RegisterAssemblyTypes(distinctAssemblies)
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
                var c = context.Resolve<IComponentContext>();
                var singleMessageHandlerRegistration = new SingleMessageHandlerRegistration();
                singleMessageHandlerRegistration.RegisterCommandHandlersByAttribute(distinctAssemblies, c.Resolve);
                return new CommandHandlerDelegateResolver(singleMessageHandlerRegistration.BuildMessageHandlerResolver());
            }).AsSelf().SingleInstance();

            return this;
        }
    }
}
