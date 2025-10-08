namespace EventBus
{
    public interface IEvent
    {
    }

    public struct TestEvent : IEvent
    {
    }

    public struct PlayerEvent : IEvent
    {
        public int health;
        public int mana;
    }

    public struct EscapeButtonPressed : IEvent {}
    public struct AnyButtonPressed : IEvent {}
}

