using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsView : View
    {
        [Header("Main Buttons")]
        [SerializeField] private Button audioButton;
        [SerializeField] private Button gameplayButton;
        [SerializeField] private Button graphicsButton;
        [SerializeField] private Button backButton;
        
        [Header("Audio Menu")]
        [SerializeField] private GameObject audioMenu;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Toggle muteToggle;
        
        [Header("Gameplay Menu")]
        [SerializeField] private GameObject gameplayMenu;
        [SerializeField] private Toggle showHintsToggle;
        
        [Header("Graphics Menu")]
        [SerializeField] private GameObject graphicsMenu;
        [SerializeField] private Toggle fullscreenToggle;
        
        public GameObject AudioMenu => audioMenu;
        public GameObject GraphicsMenu => graphicsMenu;
        public GameObject GameplayMenu => gameplayMenu;
        
        private SettingsViewModel viewModel;

        public override void Init(ViewModel vm)
        {
            base.Init(vm);
            
            viewModel = vm as SettingsViewModel;
            
            audioButton.onClick.AddListener(() => viewModel.OnAudioClicked());
            gameplayButton.onClick.AddListener(() => viewModel.OnGameplayClicked());
            graphicsButton.onClick.AddListener(() => viewModel.OnGraphicsClicked());
            backButton.onClick.AddListener(() => viewModel.OnBackClicked());
            volumeSlider.onValueChanged.AddListener(value => viewModel.OnVolumeChanged(value));
            muteToggle.onValueChanged.AddListener(isMuted => viewModel.OnMuteChanged(isMuted));
        }

        public void HideAllSubmenus()
        {
            audioMenu.SetActive(false);
            gameplayMenu.SetActive(false);
            graphicsMenu.SetActive(false);
        }

        public bool IsAnySubmenuOpen()
        {
            return audioMenu.activeSelf || gameplayMenu.activeSelf || graphicsMenu.activeSelf;
        }
    }
}