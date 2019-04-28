using System.Collections.Generic;
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
    
    public abstract class BaseCommandHandler<TCommand> where TCommand : class
    {
        private readonly List<TCommand> handledCommands = new List<TCommand>();

        public bool HasHandledCommand(TCommand command)
        {
            return handledCommands.Contains(command);
        }
        
        public virtual Task HandleAsync(TCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            handledCommands.Add(command);
            return Task.CompletedTask;
        }
    }

    public class TestCommandHandlerWithInterface : BaseCommandHandler<TestCommand>, ICommandAsyncHandler<TestCommand>
    {
    }

    public class TestCommandHandlerWithAttribute : BaseCommandHandler<TestCommand>
    {
        [CommandHandler]
        public override Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.HandleAsync(command, cancellationToken);
        }
    }

    public class MultipleCommandHandlerWithAttribute1 : BaseCommandHandler<MultipleCommand>
    {
        [CommandHandler]
        public override Task HandleAsync(MultipleCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.HandleAsync(command, cancellationToken);
        }
    }

    public class MultipleCommandHandlerWithAttribute2 : BaseEventHandler<MultipleCommand>
    {
        [CommandHandler]
        public override Task HandleAsync(MultipleCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.HandleAsync(command, cancellationToken);
        }
    }
}