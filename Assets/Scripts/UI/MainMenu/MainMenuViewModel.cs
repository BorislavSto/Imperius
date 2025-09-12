using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class MainMenuViewModel : ViewModel
    {
        private MainMenuView view;
        private MainMenuModel model;
        
        private bool showingSettings;

        private EventBinding<EscapeButtonPressed> escapeButtonPressedBinding;
        
        public MainMenuViewModel(MainMenuView view, MainMenuModel model) : base(view, model)
        {
            this.view = view;
            this.model = model;
            view.Init(this);
            
            escapeButtonPressedBinding = new EventBinding<EscapeButtonPressed>(OnEscapeTriggered);
            EventBus<EscapeButtonPressed>.Register(escapeButtonPressedBinding);
        }

        // THIS HAS TO BE REDONE! as unity's GC is non-deterministic it might be cleaned up later than id want it to!!
        ~MainMenuViewModel()
        {
            EventBus<EscapeButtonPressed>.Deregister(escapeButtonPressedBinding);
        }

        public void OnStartTriggered()
        {
            Debug.Log("Start Game clicked! Selected option: " + model.SelectedOption);
        }

        public void OnSettingsTriggered()
        {
            Debug.Log($"Settings clicked! {showingSettings!}");
         
            showingSettings = !showingSettings;
            view.SetSettingsVisible(showingSettings);
        }

        public void OnEscapeTriggered()
        {
            // close open menu/ go to main menu/ try to quit game
        }
        
        public void OnQuitTriggered()
        {
            Debug.Log("Quit clicked!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
