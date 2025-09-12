using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class Model
    {
    }

    public class View : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = 0.25f;
        private CanvasGroup canvasGroup;

        public virtual void Init(ViewModel viewModel)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        public virtual void Show()
        {
            FadeIn();
        }

        public virtual void Hide()
        {
            FadeOut();
        }

        public virtual void FadeIn()
        {
            canvasGroup.DOKill();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(1f, fadeDuration);
        }

        public virtual void FadeOut()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(OnFadeOutCompleted);
        }

        protected void OnFadeOutCompleted()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }
    }

    public class ViewModel
    {
        public View View { get; private set; }
        public Model Model { get; private set; }

        protected ViewModel(View view, Model model)
        {
            View = view;
            Model = model;
        }
    }
}
