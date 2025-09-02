using UnityEngine;

namespace UI
{
    public class MainMenuViewModel : ViewModel
    {
        private MainMenuView view;
        private MainMenuModel model;

        public MainMenuViewModel(MainMenuView view, MainMenuModel model) : base(view, model)
        {
            this.view = view;
            this.model = model;
            view.Init(this);
        }

        public void OnStartClicked()
        {
            Debug.Log("Start Game clicked! Selected option: " + model.SelectedOption);
            // Could trigger UIManager or GameManager to switch scenes
        }

        public void OnOptionsClicked()
        {
            Debug.Log("Options clicked!");
            // Could open options menu
        }

        public void OnQuitClicked()
        {
            Debug.Log("Quit clicked!");
        }
    }
}
