namespace FoodGrabber.Shared.Events;

public interface IDomainEvent { }

public interface IEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken ct = default);
}
