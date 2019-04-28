using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xer.Cqrs.EventStack;
using Xer.Cqrs.EventStack.Extensions.Attributes;

namespace Xer.Cqrs.Extensions.Autofac.Tests.Entities
{
    public class TestEvent
    {
        
    }

    public abstract class BaseEventHandler<TEvent>
    {
        private readonly List<TEvent> handledEvents = new List<TEvent>();

        public bool HasHandledEvent(TEvent @event) => handledEvents.Contains(@event);
        
        public virtual Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default)
        {
            handledEvents.Add(@event);
            return Task.CompletedTask;
        }
    }

    public class TestEventHandlerWithInterface1 : BaseEventHandler<TestEvent>, IEventAsyncHandler<TestEvent>
    {
    }

    public class TestEventHandlerWithInterface2 : BaseEventHandler<TestEvent>, IEventAsyncHandler<TestEvent>
    {
    }

    public class TestEventHandlerWithInterface3 : BaseEventHandler<TestEvent>, IEventHandler<TestEvent>
    {
        public void Handle(TestEvent @event)
        {
            HandleAsync(@event).GetAwaiter().GetResult();
        }
    }

    public class TestEventHandlerWithInterface4 : BaseEventHandler<TestEvent>, IEventHandler<TestEvent>
    {
        public void Handle(TestEvent @event)
        {
            HandleAsync(@event).GetAwaiter().GetResult();
        }
    }

    public class TestEventHandlerWithAttribute1 : BaseEventHandler<TestEvent>
    {
        [EventHandler]
        public override Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.HandleAsync(@event, cancellationToken);
        }
    }
    

    public class TestEventHandlerWithAttribute2 : BaseEventHandler<TestEvent>
    {
        [EventHandler]
        public override Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.HandleAsync(@event, cancellationToken);
        }
    }
}