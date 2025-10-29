using Core;
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

            InputHandler.CancelPressedEvent += OnEscapeButtonPressed;
            InputHandler.AnyPressedEvent += OnAnyButtonPressed;
        } 
        
        private void OnDestroy()
        {
            InputHandler.CancelPressedEvent -= OnEscapeButtonPressed;
            InputHandler.AnyPressedEvent -= OnAnyButtonPressed;
        }

        private void OnEscapeButtonPressed()
        {
            EventBus<EscapeButtonPressed>.Raise(new EscapeButtonPressed());
        }

        private void OnAnyButtonPressed()
        {
            EventBus<AnyButtonPressed>.Raise(new AnyButtonPressed());
        }
    }
}