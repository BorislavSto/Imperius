using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MainMenuSystem : MonoBehaviour
    {
        [SerializeField] private MainMenuView mainMenuView;
        [SerializeField] private EventSystem eventSystem;
        private MainMenuViewModel mainMenuViewModel;

        void Awake()
        {
            mainMenuViewModel = new MainMenuViewModel(mainMenuView, new MainMenuModel());
        }
    }
}