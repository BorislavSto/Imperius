using System;
using Core;
using UnityEngine;

namespace UI
{
    public class MainMenuViewModel : ViewModel
    {
        private readonly MainMenuView view;
        private readonly MainMenuModel model;
        
        private Action escapeAction;
        
        public MainMenuViewModel(MainMenuView view, MainMenuModel model)
        {
            this.view = view;
            this.model = model;
            view.Init(this);
        }

        public void OnStartTriggered()
        {
            // Gameplay UI has to be loaded additively before the gameplay scene to allow proper initialization
            SceneManager.Instance.LoadMultipleScenesAndUnloadCurrent(
                new [] { SceneNames.SceneGameplayUI, SceneNames.SceneGameplay }, true);
            
            Debug.Log("Start Game clicked! Selected option: " + model.SelectedOption);
        }

        public void OnSettingsTriggered()
        {
            UIManager.Instance.SettingsSystem.OpenSettings();
        }

        protected override void OnEscapeTriggered()
        {
            OnQuitTriggered();
        }
        
        public void OnQuitTriggered()
        {
            UIManager.Instance.PopupSystem.ShowPopup(
                "Are you sure you want to quit?",
                new (string, Action)[] {
                    ("Yes", Application.Quit),
                    ("No", null)
                }
            );
        }
    }
}
