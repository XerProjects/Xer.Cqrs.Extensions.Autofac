using System;
using System.Threading;
using System.Threading.Tasks;
using Xer.Cqrs.CommandStack;

namespace Xer.Xqrs.Extensions.Autofac.Tests
{
    internal class MultipleCommandHandler2 : ICommandAsyncHandler<MultipleCommand>
    {
        public Task HandleAsync(MultipleCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}