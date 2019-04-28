using Autofac;
using Xer.Cqrs.CommandStack;
using Xunit;
using Xer.Cqrs.Extensions.Autofac.Tests.Entities;
using Xer.Cqrs.EventStack;
using System.Collections.Generic;
using System;
using FluentAssertions;
using System.Threading.Tasks;
using System.Linq;

namespace Xer.Cqrs.Extensions.Autofac.Tests
{
    public class ContainerBuilderExtensionsTests
    {
        public class RegisterCqrsMethods
        {
            [Fact]
            public void ShouldResolveCommandDelegator()
            {
                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterCqrsCore()
                    .RegisterCommandHandlers(opt => opt.ByInterface(typeof(TestCommand).Assembly));

                using (IContainer context = builder.Build())
                {
                    context.Resolve<CommandDelegator>().Should().NotBeNull();
                }
            }

            [Fact]
            public void ShouldNotResolveCommandHandlerResolver()
            {
                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterCqrsCore();

                using (IContainer context = builder.Build())
                {
                    context.Resolve<IEnumerable<CommandHandlerDelegateResolver>>().Should().BeEmpty();
                }
            }

            [Fact]
            public void ShouldResolveCommandHandlerByInterface()
            {
                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterCqrsCore()
                    .RegisterCommandHandlers(opt => opt.ByInterface(typeof(TestCommand).Assembly));

                using (IContainer context = builder.Build())
                {
                    var commandHandler = context.Resolve<ICommandAsyncHandler<TestCommand>>();

                    commandHandler.Should().BeOfType<TestCommandHandlerWithInterface>();
                }
            }

            [Fact]
            public void ShouldResolveCommandHandlerByAttribute()
            {
                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterCqrsCore()
                    .RegisterCommandHandlers(opt => opt.ByAttribute(typeof(TestCommand).Assembly));

                using (IContainer context = builder.Build())
                {
                    context.Resolve<TestCommandHandlerWithAttribute>().Should().NotBeNull();
                }
            }

            [Fact]
            public void ShouldResolveMultipleCommandHandlersByAttribute()
            {
                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterCqrsCore()
                    .RegisterCommandHandlers(opt => opt.ByAttribute(typeof(TestCommand).Assembly));

                using (IContainer context = builder.Build())
                {
                    context.Resolve<MultipleCommandHandlerWithAttribute1>().Should().NotBeNull();
                    context.Resolve<MultipleCommandHandlerWithAttribute2>().Should().NotBeNull();
                }
            }

            [Fact]
            public void ShouldResolveEventDelegator()
            {
                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterCqrsCore()
                    .RegisterEventHandlers(opt => opt.ByAttribute(typeof(TestEvent).Assembly));

                using (IContainer context = builder.Build())
                {
                    context.Resolve<EventDelegator>().Should().NotBeNull();
                }
            }

            [Fact]
            public void ShouldResolveMultipleEventHandlersByAttribute()
            {
                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterCqrsCore()
                    .RegisterEventHandlers(opt => opt.ByAttribute(typeof(TestEvent).Assembly));

                using (IContainer context = builder.Build())
                {
                    context.Resolve<TestEventHandlerWithAttribute1>().Should().NotBeNull();
                    context.Resolve<TestEventHandlerWithAttribute2>().Should().NotBeNull();
                }
            }

            [Fact]
            public void ShouldResolveEventHandlersByInterface()
            {
                ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterCqrsCore()
                    .RegisterEventHandlers(opt => opt.ByInterface(typeof(TestEvent).Assembly));

                using(IContainer context = builder.Build())
                {
                    var eventHandlers = context.Resolve<IEnumerable<IEventAsyncHandler<TestEvent>>>();

                    eventHandlers.Should().NotBeEmpty();

                    context.Resolve<TestEventHandlerWithInterface1>().Should().NotBeNull();
                    context.Resolve<TestEventHandlerWithInterface2>().Should().NotBeNull();
                }
            }

            [Fact]
            public void ShouldThrowWhenNoAssembliesAreProvided()
            {
                ContainerBuilder builder = new ContainerBuilder();
                Action action = () => builder.RegisterCqrs();

                action.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void ShouldThrowWhenNullAssemblyIsProvided()
            {
                ContainerBuilder builder = new ContainerBuilder();
                Action action = () => builder.RegisterCqrs(null);

                action.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldThrowWhenNoCommandHandlerAssembliesAreProvidedInByInterface()
            {
                ContainerBuilder builder = new ContainerBuilder();
                Action action = () => builder.RegisterCqrsCore()
                                            .RegisterCommandHandlers(opt => opt.ByInterface());

                action.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void ShouldThrowWhenNoEventHandlerAssembliesAreProvidedInByInterface()
            {
                ContainerBuilder builder = new ContainerBuilder();
                Action action = () => builder.RegisterCqrsCore()
                                            .RegisterEventHandlers(opt => opt.ByInterface());

                action.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void ShouldThrowWhenNoCommandHandlerAssembliesAreProvidedInByAttribute()
            {
                ContainerBuilder builder = new ContainerBuilder();
                Action action = () => builder.RegisterCqrsCore()
                                            .RegisterCommandHandlers(opt => opt.ByAttribute());

                action.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void ShouldThrowWhenNoEventHandlerAssembliesAreProvidedInByAttribute()
            {
                ContainerBuilder builder = new ContainerBuilder();
                Action action = () => builder.RegisterCqrsCore()
                                            .RegisterEventHandlers(opt => opt.ByAttribute());

                action.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void ShouldThrowWhenNullCommandHandlerAssemblyIsProvidedInByAttribute()
            {
                ContainerBuilder builder = new ContainerBuilder();
                Action action = () => builder.RegisterCqrsCore()
                                            .RegisterCommandHandlers(opt => opt.ByAttribute(null));

                action.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldThrowWhenNullEventHandlerAssemblyIsProvidedInByAttribute()
            {
                ContainerBuilder builder = new ContainerBuilder();
                Action action = () => builder.RegisterCqrsCore()
                                            .RegisterEventHandlers(opt => opt.ByAttribute(null));

                action.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldThrowWhenNullCommandHandlerAssemblyIsProvidedInByInterface()
            {
                ContainerBuilder builder = new ContainerBuilder();
                Action action = () => builder.RegisterCqrsCore()
                                            .RegisterCommandHandlers(opt => opt.ByInterface(null));

                action.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldThrowWhenNullEventHandlerAssemblyIsProvidedInByInterface()
            {
                ContainerBuilder builder = new ContainerBuilder();
                Action action = () => builder.RegisterCqrsCore()
                                            .RegisterEventHandlers(opt => opt.ByInterface(null));

                action.Should().Throw<ArgumentNullException>();
            }
        }

        public class CommandDelegatorTests
        {
            public class SendAsyncMethod
            {
                [Fact]
                public async Task ShouldSendCommandToHandler()
                {
                    ContainerBuilder builder = new ContainerBuilder();
                    builder.RegisterCqrsCore()
                        .RegisterCommandHandlers(opt => opt.ByInterface(Lifetime.Singleton, typeof(TestCommand).Assembly));

                    using (IContainer context = builder.Build())
                    {
                        var commandDelegator = context.Resolve<CommandDelegator>();
                        var command = new TestCommand();
                        await commandDelegator.SendAsync(command);

                        var handler = context.Resolve<ICommandAsyncHandler<TestCommand>>();
                        handler.As<BaseCommandHandler<TestCommand>>()
                            .HasHandledCommand(command).Should().BeTrue();
                    }
                }
            }
        }

        public class EventDelegatorSendMethod
        {
            public class SendAsyncMethod
            {
                [Fact]
                public async Task ShouldSendEventToHandlers()
                {
                    ContainerBuilder builder = new ContainerBuilder();
                    builder.RegisterCqrsCore()
                        .RegisterEventHandlers(opt => opt.ByInterface(Lifetime.Singleton, typeof(TestEvent).Assembly));

                    using (IContainer context = builder.Build())
                    {
                        var eventDelegator = context.Resolve<EventDelegator>();
                        var @event = new TestEvent();
                        await eventDelegator.SendAsync(@event);

                        var handlers = context.Resolve<IEnumerable<IEventAsyncHandler<TestEvent>>>();
                        handlers.All(h => h.As<BaseEventHandler<TestEvent>>().HasHandledEvent(@event))
                            .Should().BeTrue();
                    }
                }
            }
        }
    }
}
