using System.Threading;
using System.Threading.Tasks;
using Xer.Cqrs.CommandStack.Attributes;

namespace Xer.Xqrs.Extensions.Autofac.Tests
{
    internal class MultipleCommandHandlerWithAttribute2
    {
        [CommandHandler]
        public Task HandleAsync(MultipleCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}