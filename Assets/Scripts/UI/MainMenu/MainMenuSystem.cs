using UnityEngine;

namespace UI
{
    public class MainMenuSystem : MvvmSystem<MainMenuViewModel>
    {
        [SerializeField] private MainMenuView mainMenuView;

        void Awake()
        {
            viewModel = new MainMenuViewModel(mainMenuView, new MainMenuModel());
        }
    }
}