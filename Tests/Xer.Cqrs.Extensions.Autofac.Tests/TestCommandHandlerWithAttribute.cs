using System.Threading;
using System.Threading.Tasks;
using Xer.Cqrs.CommandStack.Attributes;

namespace Xer.Xqrs.Extensions.Autofac.Tests
{
    internal class TestCommandHandlerWithAttribute
    {
        [CommandHandler]
        public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}