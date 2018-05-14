using System.Threading;
using System.Threading.Tasks;
using Xer.Cqrs.EventStack;
using Xer.Cqrs.EventStack.Extensions.Attributes;

namespace Xer.Cqrs.Extensions.Autofac.Tests.Entities
{
    public class TestEvent
    {
        
    }
    
    public class TestEventHandlerWithInterface1 : IEventAsyncHandler<TestEvent>
    {
        public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }

    public class TestEventHandlerWithInterface2 : IEventAsyncHandler<TestEvent>
    {
        public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }

    public class TestEventHandlerWithAttribute1
    {
        [EventHandler]
        public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
    

    public class TestEventHandlerWithAttribute2
    {
        [EventHandler]
        public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}