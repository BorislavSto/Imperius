using UnityEngine;

namespace UI
{
    public class MainMenuSystem : MonoBehaviour
    {
        [SerializeField] private MainMenuView mainMenuView;
        private MainMenuViewModel mainMenuViewModel;

        void Awake()
        {
            mainMenuViewModel = new MainMenuViewModel(mainMenuView, new MainMenuModel());
        }
    }
}