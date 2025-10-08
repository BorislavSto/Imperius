using UnityEngine;

namespace UI
{
    public class SettingsSystem : MonoBehaviour
    {
        [SerializeField] private SettingsView view;

        private SettingsViewModel viewModel;

        private void Awake()
        {
            viewModel = new SettingsViewModel(view, new SettingsModel());
            view.HideSettings();
        }

        public void OpenSettings()
        {
            view.ShowSettings();
        }
    }
}