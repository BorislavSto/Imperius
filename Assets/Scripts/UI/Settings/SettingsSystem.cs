using UnityEngine;

namespace UI
{
    public class SettingsSystem : MvvmSystem<SettingsViewModel>
    {
        [SerializeField] private SettingsView view;

        private void Awake()
        {
            viewModel = new SettingsViewModel(view, new SettingsModel());
            viewModel.HideSettings();
        }

        public void OpenSettings()
        {
            viewModel.ShowSettings();
        }
    }
}