using Autofac;
using System.Reflection;
using Autofac.Core.Registration;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.Extensions.Autofac;
using Xunit;

namespace Xer.Xqrs.Extensions.Autofac.Tests
{
    public class CommandHandlerTests
    {
        [Fact]
        public void Should_Resolve_CommandDelegator()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterCommandHandlers(select => select.ByInterface(typeof(TestCommand).Assembly));

            var context = builder.Build();

            Assert.NotNull(context.Resolve<CommandDelegator>());
        }

        [Fact]
        public void Should_Notresolve_CommandHandlerResolver()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore();

            var context = builder.Build();

            Assert.Throws<ComponentNotRegisteredException>(() => context.Resolve<CommandHandlerDelegateResolver>());
        }

        [Fact]
        public void Should_Resolve_CommandHandler_ByInterface()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterCommandHandlers(opt => opt.ByInterface(typeof(TestCommand).Assembly));

            var context = builder.Build();

            var commandHandler = context.Resolve<ICommandAsyncHandler<TestCommand>>();

            Assert.IsType<TestCommandHandlerWithInterface>(commandHandler);
        }

        [Fact]
        public void Should_Resolve_CommandHandler_ByAttribute()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterCommandHandlers(opt => opt.ByAttribute(typeof(TestCommand).Assembly));

            var context = builder.Build();

            Assert.NotNull(context.Resolve<TestCommandHandlerWithAttribute>());
        }

        [Fact]
        public void Should_Resolve_Multiple_CommandHandlers_ByAttribute()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterCqrsCore()
                .RegisterCommandHandlers(opt => opt.ByAttribute(typeof(TestCommand).Assembly));

            var context = builder.Build();

            Assert.NotNull(context.Resolve<MultipleCommandHandlerWithAttribute1>());
            Assert.NotNull(context.Resolve<MultipleCommandHandlerWithAttribute2>());
        }
    }
}
