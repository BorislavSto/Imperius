using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuView : View
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        private MainMenuViewModel viewModel;

        public override void Init(ViewModel vm)
        {
            base.Init(vm);
            
            viewModel = vm as MainMenuViewModel;

            startButton.onClick.AddListener(() => viewModel.OnStartTriggered());
            settingsButton.onClick.AddListener(() => viewModel.OnSettingsTriggered());
            quitButton.onClick.AddListener(() => viewModel.OnQuitTriggered());
            
            ShowView();
        }
    }
}
