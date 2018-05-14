using System.Threading;
using System.Threading.Tasks;
using Xer.Cqrs.CommandStack;
using Xer.Cqrs.CommandStack.Extensions.Attributes;

namespace Xer.Cqrs.Extensions.Autofac.Tests.Entities
{
    public class MultipleCommand
    {

    }

    public class TestCommand
    {

    }

    public class TestCommandHandlerWithAttribute
    {
        [CommandHandler]
        public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }

    public class TestCommandHandlerWithInterface : ICommandAsyncHandler<TestCommand>
    {
        public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }

    public class MultipleCommandHandlerWithAttribute1
    {
        [CommandHandler]
        public Task HandleAsync(MultipleCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }

    public class MultipleCommandHandlerWithAttribute2
    {
        [CommandHandler]
        public Task HandleAsync(MultipleCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}