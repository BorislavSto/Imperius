using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuView : View
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private GameObject settingsTab;

        private MainMenuViewModel viewModel;

        public override void Init(ViewModel vm)
        {
            base.Init(vm);
            
            viewModel = vm as MainMenuViewModel;

            startButton.onClick.AddListener(() => viewModel.OnStartTriggered());
            settingsButton.onClick.AddListener(() => viewModel.OnSettingsTriggered());
            quitButton.onClick.AddListener(() => viewModel.OnQuitTriggered());
            
            Show();
        }
        
        public void SetSettingsVisible(bool visible)
        {
            settingsTab.SetActive(visible);
        }
        
        public override void Show()
        {
            base.Show();
            Debug.Log("Main Menu Shown");
        }

        public override void Hide()
        {
            base.Hide();
            Debug.Log("Main Menu Hidden");
        }
    }
}
