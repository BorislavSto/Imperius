using EventBus;

namespace Player
{
    public class InputManager : Singleton<InputManager>
    {
        public PlayerInputHandler InputHandler { get; private set;}
        
        protected override void Awake()
        {
            base.Awake();
            InputHandler = gameObject.AddComponent<PlayerInputHandler>();
            
            InputHandler.CancelPressedEvent += () =>
            {
                EventBus<EscapeButtonPressed>.Raise(new EscapeButtonPressed());
            };
        }
    }
}