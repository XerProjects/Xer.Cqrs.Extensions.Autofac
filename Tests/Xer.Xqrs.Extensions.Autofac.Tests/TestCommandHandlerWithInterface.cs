using System.Threading;
using System.Threading.Tasks;
using Xer.Cqrs.CommandStack;

namespace Xer.Xqrs.Extensions.Autofac.Tests
{
    internal class TestCommandHandlerWithInterface : ICommandAsyncHandler<TestCommand>
    {
        public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}