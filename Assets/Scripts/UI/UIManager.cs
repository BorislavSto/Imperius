using System;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.EventSystems;
using Core;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private SettingsSystem settingsSystem;
        [SerializeField] private PopupSystem popupSystem;
        [SerializeField] private EventSystem eventSystem;
        
        public SettingsSystem SettingsSystem => settingsSystem;
        public PopupSystem PopupSystem => popupSystem;
        
        private Stack<Action> escapeActions = new();
        public Action<bool> TriggerLoading; 
        private EventBinding<EscapeButtonPressed> escapeBinding;
        private EventBinding<SceneLoadingEvent> loadingBinding;
        private GameObject lastSelectedUIElement;

        protected override void Awake()
        {
            base.Awake();
            escapeBinding = new EventBinding<EscapeButtonPressed>(OnEscapePressed);
            loadingBinding = new EventBinding<SceneLoadingEvent>(OnLoadingTriggered);
            EventBus<EscapeButtonPressed>.Register(escapeBinding);
            EventBus<SceneLoadingEvent>.Register(loadingBinding);
        }


        private void OnDestroy()
        {
            EventBus<EscapeButtonPressed>.Deregister(escapeBinding);
            EventBus<SceneLoadingEvent>.Deregister(loadingBinding);
        }

        public void PushEscapeAction(Action action)
        {
            if (!escapeActions.Contains(action)) 
                escapeActions.Push(action);
        }

        public void PopEscapeAction(Action action)
        {
            if (escapeActions.Count > 0 && escapeActions.Peek() == action)
                escapeActions.Pop();
        }

        private void OnEscapePressed()
        {
            Debug.Log($"pressing escape, escape actions amount = {escapeActions.Count}?");
            if (escapeActions.Count > 0)
                escapeActions.Peek().Invoke();
            else
                Debug.Log("Escape pressed with no menus open → quit?");
        }
        
        public void SetSelectedUIElement(GameObject uiElement)
        {
            if (uiElement == null)
                return;
            
            lastSelectedUIElement = eventSystem.currentSelectedGameObject;
            eventSystem.SetSelectedGameObject(uiElement);
        }
        
        public void ShowPreviouslySelectedUIElement()
        {
            eventSystem.SetSelectedGameObject(lastSelectedUIElement);
            lastSelectedUIElement = null;
        }
        
        private void OnLoadingTriggered(SceneLoadingEvent obj)
        {
            TriggerLoading?.Invoke(obj.IsLoading);
        }
    }
}