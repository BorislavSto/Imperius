using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public abstract class Model
    {
        public abstract void Load();
        public abstract void Save();
    }

    public class View : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = 0.25f;
        
        private CanvasGroup canvasGroup;
        private ViewModel viewModel;

        public virtual void Init(ViewModel vm)
        {
            viewModel = vm;
            
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            
            Hide(0f);
        }

        protected void Show(float? fadeTime = null)
        {
            float duration = fadeTime ?? fadeDuration;
            FadeIn(duration);
            UpdateUIElements();
            viewModel.SubscribeToEscapeEvent();
        }
        
        protected void Hide(float? fadeTime = null)
        {
            float duration = fadeTime ?? fadeDuration;
            FadeOut(duration);
            CleanupUIElements();
            viewModel.UnsubscribeFromEscapeEvent();
        }

        private void UpdateUIElements()
        {
            ViewUIElement[] uiElements = GetComponentsInChildren<ViewUIElement>(true);

            foreach (ViewUIElement element in uiElements)
                element.UpdateUIElement();
        }
        
        private void CleanupUIElements()
        {
            ViewUIElement[] uiElements = GetComponentsInChildren<ViewUIElement>(true);
            
            foreach (ViewUIElement element in uiElements)
                element.CleanupUIElement();
        }

        private void FadeIn(float fadeTime)
        {
            canvasGroup.DOKill();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(1f, fadeTime).SetLink(canvasGroup.gameObject);
        }

        private void FadeOut(float fadeTime)
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeTime).OnComplete(OnFadeOutCompleted).SetLink(canvasGroup.gameObject);
        }

        private void OnFadeOutCompleted()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        
        public void ShowView() => Show();
        public void HideView() => Hide();
    }

    public class ViewModel
    {
        private readonly Action escapeAction;

        protected ViewModel()
        {
            escapeAction = OnEscapeTriggered;
        }

        // triggered on Show in View
        public void SubscribeToEscapeEvent()
        {
            UIManager.Instance.PushEscapeAction(escapeAction);
        }

        // triggered on Hide in View
        public void UnsubscribeFromEscapeEvent()
        {
            UIManager.Instance.PopEscapeAction(escapeAction);
        }

        public virtual void Dispose()
        {
            UIManager.Instance.PopEscapeAction(escapeAction);
        }
        
        protected virtual void OnEscapeTriggered() {}
    }

    public class MvvmSystem<T> : MonoBehaviour where T : ViewModel
    {
        protected T viewModel;
        
        private void OnDestroy()
        {
            viewModel?.Dispose();
        }
    }
}
