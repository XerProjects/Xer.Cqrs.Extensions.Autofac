using Autofac;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Core.Registration;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.Extensions.Autofac;
using Xunit;

namespace Xer.Xqrs.Extensions.Autofac.Tests
{
    public class CommandHandlerTests
    {
        private readonly Assembly _assembly;

        public CommandHandlerTests()
        {
            this._assembly = typeof(TestCommand).Assembly;
        }

        [Fact]
        public void Should_Resolve_CommandDelegator()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.AddCqrsCore()
                .AddCommandHandlers();

            var context = builder.Build();

            Assert.NotNull(context.Resolve<CommandDelegator>());
        }

        [Fact]
        public void Should_Notresolve_CommandHandlerResolver()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.AddCqrsCore()
                .AddCommandHandlers();

            var context = builder.Build();

            Assert.Throws<ComponentNotRegisteredException>(() => context.Resolve<CommandHandlerDelegateResolver>());
        }

        [Fact]
        public void Should_Resolve_CommandHandler_ByInterface()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.AddCqrsCore()
                .AddCommandHandlers(opt => opt.ByInterface(_assembly));

            var context = builder.Build();

            var commandHandler = context.Resolve<ICommandAsyncHandler<TestCommand>>();

            Assert.IsType<TestCommandHandlerWithInterface>(commandHandler);
        }

        [Fact]
        public void Should_Resolve_CommandHandler_ByAttribute()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.AddCqrsCore()
                .AddCommandHandlers(opt => opt.ByAttribute(_assembly));

            var context = builder.Build();

            Assert.NotNull(context.Resolve<TestCommandHandlerWithAttribute>());
        }

        [Fact]
        public void Should_Resolve_Multiple_CommandHandlers_ByInterface()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.AddCqrsCore()
                .AddCommandHandlers(opt => opt.ByInterface(_assembly));

            var context = builder.Build();

            var commandHandler = context.Resolve<IEnumerable<ICommandAsyncHandler<MultipleCommand>>>();

            Assert.Equal(2, commandHandler.Count());
        }

        [Fact]
        public void Should_Resolve_Multiple_CommandHandlers_ByAttribute()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.AddCqrsCore()
                .AddCommandHandlers(opt => opt.ByAttribute(_assembly));

            var context = builder.Build();

            Assert.NotNull(context.Resolve<MultipleCommandHandlerWithAttribute1>());
            Assert.NotNull(context.Resolve<MultipleCommandHandlerWithAttribute2>());
        }
    }
}
