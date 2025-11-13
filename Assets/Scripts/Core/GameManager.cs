using UnityEngine;

namespace Core
{
    public class GameManager : LocalSingleton<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
