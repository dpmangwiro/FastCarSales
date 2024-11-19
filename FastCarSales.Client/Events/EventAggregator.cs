namespace FastCarSales.Client.Events
{
    public class EventAggregator
    {
        private readonly List<Subscription> _subscriptions = new List<Subscription>();

        public void Subscribe<TEvent>(Action<TEvent> action, string? eventTopic = null)
        {
            _subscriptions.Add(new Subscription(typeof(TEvent), o => action((TEvent)o), eventTopic));
        }

        public void publish<TEvent>(TEvent @event, string? eventTopic = null)
        {
            if (@event == null) { return; }

            foreach (var subscription in _subscriptions.Where(s => s.EventType == typeof(TEvent) && s.EventTopic == eventTopic))
            {
                subscription.Action(@event);
            }
        }

        private class Subscription
        {
            public Type EventType { get; }
            public Action<object> Action { get; }
            public string? EventTopic { get; }

            public Subscription(Type eventType, Action<object> action, string? eventTopic = null)
            {
                EventType = eventType;
                Action = action;
                EventTopic = eventTopic;
            }
        }





    }
}
