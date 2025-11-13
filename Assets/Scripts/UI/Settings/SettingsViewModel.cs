using System;
using Core;
using UnityEngine;

namespace UI
{
    public class SettingsViewModel : ViewModel
    {
        private readonly SettingsView view;
        private readonly SettingsModel model;
        
        private Action escapeAction;
        
        public SettingsViewModel(SettingsView view, SettingsModel model) : base()
        {
            this.view = view;
            this.model = model;
            
            model.Load();
            
            view.Init(this);
        }

        public void ShowSettings()
        {
            view.ShowView();
        }
        
        public void HideSettings()
        {
            view.HideView();
        }
        
        public void OnAudioClicked()
        {
            view.HideAllSubmenus();
            view.AudioMenu.SetActive(true);
            Debug.Log("Audio submenu opened");
        }

        public void OnVolumeChanged(float value)
        {
            model.volume = value;
            Debug.Log($"Volume changed {value}");
            AudioManager.Instance.AudioMixer.SetFloat("MasterVolume", value);
        }

        public void OnMuteChanged(bool isMuted)
        {
            model.muted = isMuted;
            AudioManager.Instance.AudioMixer.SetFloat("MasterVolume", isMuted ? -80f : 0f);
        }

        public void OnGameplayClicked()
        {
            view.HideAllSubmenus();
            view.GameplayMenu.SetActive(true);
        }

        public void OnGraphicsClicked()
        {
            view.HideAllSubmenus();
            view.GraphicsMenu.SetActive(true);
        }
        
        public void OnBackClicked()
        {
            // reuse same logic as escape/back-stack
            HandleBack(); 
        }

        protected override void OnEscapeTriggered()
        {
            HandleBack();
        }

        private void HandleBack()
        {
            if (view.IsAnySubmenuOpen())
            {
                view.HideAllSubmenus();
                return;
            }

            CloseSettings();
        }
        
        private void CloseSettings()
        {
            Debug.Log($"Settings closed");
            view.HideAllSubmenus();
            view.HideView();
        }
    }
}