using Player.Input;

namespace Core
{
    public class InputManager : Singleton<InputManager>
    {
        public PlayerInputHandler InputHandler { get; private set;}

        protected override void Awake()
        {
            base.Awake();
            InputHandler = gameObject.AddComponent<PlayerInputHandler>();
        }
    }
}