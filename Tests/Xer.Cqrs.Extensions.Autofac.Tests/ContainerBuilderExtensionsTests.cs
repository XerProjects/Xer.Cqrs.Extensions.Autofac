using Autofac;
using Xer.Cqrs.CommandStack;
using Xunit;
using Xer.Cqrs.Extensions.Autofac.Tests.Entities;
using Xer.Cqrs.EventStack;
using System.Collections.Generic;
using System;
using FluentAssertions;

namespace Xer.Cqrs.Extensions.Autofac.Tests
{
    public class ContainerBuilderExtensionsTests
    {
        [Fact]
        public void ShouldResolveCommandDelegator()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterCommandHandlers(opt => opt.ByInterface(typeof(TestCommand).Assembly));

            var context = builder.Build();

            context.Resolve<CommandDelegator>().Should().NotBeNull();
        }

        [Fact]
        public void ShouldNotResolveCommandHandlerResolver()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore();

            var context = builder.Build();

            context.Resolve<IEnumerable<CommandHandlerDelegateResolver>>().Should().BeEmpty();
        }

        [Fact]
        public void ShouldResolveCommandHandlerByInterface()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterCommandHandlers(opt => opt.ByInterface(typeof(TestCommand).Assembly));

            var context = builder.Build();

            var commandHandler = context.Resolve<ICommandAsyncHandler<TestCommand>>();

            commandHandler.Should().BeOfType<TestCommandHandlerWithInterface>();
        }

        [Fact]
        public void ShouldResolveCommandHandlerByAttribute()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterCommandHandlers(opt => opt.ByAttribute(typeof(TestCommand).Assembly));

            var context = builder.Build();

            context.Resolve<TestCommandHandlerWithAttribute>().Should().NotBeNull();
        }

        [Fact]
        public void ShouldResolveMultipleCommandHandlersByAttribute()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterCommandHandlers(opt => opt.ByAttribute(typeof(TestCommand).Assembly));

            var context = builder.Build();

            context.Resolve<MultipleCommandHandlerWithAttribute1>().Should().NotBeNull();
            context.Resolve<MultipleCommandHandlerWithAttribute2>().Should().NotBeNull();
        }

        [Fact]
        public void ShouldResolveEventDelegator()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterEventHandlers(opt => opt.ByAttribute(typeof(TestEvent).Assembly));

            var context = builder.Build();

            context.Resolve<EventDelegator>().Should().NotBeNull();
        }

        [Fact]
        public void ShouldResolveMultipleEventHandlersByAttribute()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterEventHandlers(opt => opt.ByAttribute(typeof(TestEvent).Assembly));

            var context = builder.Build();

            context.Resolve<TestEventHandlerWithAttribute1>().Should().NotBeNull();
            context.Resolve<TestEventHandlerWithAttribute2>().Should().NotBeNull();
        }

        [Fact]
        public void ShouldResolveEventHandlersByInterface()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterEventHandlers(opt => opt.ByInterface(typeof(TestEvent).Assembly));

            var context = builder.Build();

            var eventHandlers = context.Resolve<IEnumerable<IEventAsyncHandler<TestEvent>>>();

            eventHandlers.Should().NotBeEmpty();

            context.Resolve<TestEventHandlerWithInterface1>().Should().NotBeNull();
            context.Resolve<TestEventHandlerWithInterface2>().Should().NotBeNull();
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
}
