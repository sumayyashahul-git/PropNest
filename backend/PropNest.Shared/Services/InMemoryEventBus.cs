using System.Collections.Concurrent;

namespace PropNest.Shared.Services;

public class InMemoryEventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, List<object>> _handlers = new();

    public void Publish<T>(T @event) where T : class
    {
        var eventType = typeof(T);

        if (!_handlers.TryGetValue(eventType, out var handlers)) 
            return;

        foreach (var handler in handlers)
        {
            if (handler is Action<T> typedHandler)
                typedHandler(@event);
        }
    }

    public void Subscribe<T>(Action<T> handler) where T : class
    {
        var eventType = typeof(T);

        _handlers.AddOrUpdate(
            eventType,
            _ => [handler],
            (_, existing) => { existing.Add(handler); return existing; }
        );
    }
}